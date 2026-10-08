const gulp = require("gulp");
var jp = require('jsonpath');

const $ = require("gulp-load-plugins")({DEBUG:false, lazy: true });
const lec = require("gulp-line-ending-corrector");
const print = require("gulp-print").default;

const Vinyl = require('vinyl');
const logger = require('fancy-log');
const color = require('ansi-colors');

var _ = require("underscore");
var l = require("lodash");

var path = require("path");
var fs = require("fs");
const BSON = require("bson");

var es = require("event-stream");

const fakerImport = require("./fakerImport");

var dataFolder = "../Bogus/data";
var dataExtendFolder = "../Bogus/data_extend";

function importLocalesJsonTask(cb) {
   // faker.js (TypeScript layout) -> Bogus legacy locale JSON, see fakerImport.js
   var imported = fakerImport.importAll();

   var files = fakerImport.writeLocales(imported, dataFolder, dataExtendFolder);
   files.forEach(log2);

   _.each(imported, (entry, code) => {
      entry.warnings.forEach(w => logger.warn(color.yellow(`[${code}] ${w}`)));
   });
   cb();
}

function importLocalesTask(){
   return gulp.src(`${dataFolder}/*.locale.json`)
      .pipe($.plumber())
      .pipe($.map(function (file) {
         var json = JSON.parse(file.contents.toString());

         var destName = `${path.basename(file.relative, ".json")}.bson`;

         var data = BSON.serialize(json, { checkKeys: true });

         var vinyl = new Vinyl({
            path: './' + destName,
            contents: Buffer.from(data)
         });
         return vinyl;
      }))
      .pipe(print())
      .pipe(gulp.dest(dataFolder));
}

//Helper Methods
function log(msg) {
   logger(color.bgCyan(msg));
};
function log2(msg) {
   logger(color.green(msg));
}

function importTransliterateTask(cb) {

   //strip out the module scoping of the library
   var src = fs.readFileSync('../speakingurl/lib/speakingurl.js', 'utf8');
   var lines = src.split('\n');
   var moduleEnd = _.findIndex(lines, i => i.includes("typeof module"))
   var fixedSource = lines.splice(2, moduleEnd - 2).join('\n');

   //evaluate the whole module without function scoping
   //exposing intenral variables that we can dump.
   eval(fixedSource);

   function renderInsert(obj) {
      var inserts = [];
      _.map(obj, (v, k) => {
         if (v === '"') v = '""';
         return inserts.push(`            Trie.Insert(trie, @"${k}", @"${v}");`);
      });
      return inserts;
   }
   function renderMdInsert(obj) {
      var inserts = [];
      _.map(obj, (v, k) => {
         _.map(v, (v2, k2) => {
            inserts.push(`            md.Add(@"${k}", @"${k2}", @"${v2}");`);
         })
      });
      return inserts;
   }

   var charMapInserts = renderInsert(charMap);
   var diatricMapInserts = renderInsert(diatricMap);

   var langCharInserts = renderMdInsert(langCharMap);
   var symbolInserts = renderMdInsert(symbolMap);

   var template = `
      // AUTO GENERATED FILE. DO NOT MODIFY.
      // SEE Builder/gulpfile.js import.speakingurl task.
      using System.ComponentModel;
      using System.Collections.Generic;
      namespace Bogus
      {
         
         public static partial class Transliterater
         {   
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static Trie BuildCharMap(Trie trie)
            {
   ${charMapInserts.join('\r\n')}
               return trie;
            }
   
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static Trie BuildDiatricMap(Trie trie)
            {
   ${diatricMapInserts.join('\r\n')}
               return trie;
            }
   
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static MultiDictionary<string,string,string> BuildLangCharMap(MultiDictionary<string,string,string> md)
            {
   ${langCharInserts.join('\r\n')}
               return md;
            }
   
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static MultiDictionary<string,string,string> BuildSymbolMap(MultiDictionary<string,string,string> md)
            {
   ${symbolInserts.join('\r\n')}
               return md;
            }
         }
      }
      `


   fs.writeFileSync('../Bogus/Transliterater.Generated.cs', template);

   return cb;
}


exports.importLocales = gulp.series(importLocalesJsonTask, importLocalesTask)
exports.importTransliterate = importTransliterateTask