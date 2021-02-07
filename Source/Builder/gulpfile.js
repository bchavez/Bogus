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

var localeFolders = gulp.src(
   [
      "../fakerjs/lib/locales/*",
      "!../fakerjs/lib/locales/ar", // 2018.09.23 - Exclude this locale, has problems upstream.
                                    // https://github.com/Marak/faker.js/pull/505/files#r219737439
   ]);

var dataFolder = "../Bogus/data";
var dataExtendFolder = "../Bogus/data_extend";

function importLocalesJsonTask(){
   return localeFolders
      .pipe($.plumber())
      .pipe($.map(function (file) {
         var localeCode = file.relative;
         var localeIndex = file.path + "/index.js";
         var locale = require(localeIndex);

         // Transform Step: Currencies
         transformCurrency(locale);

         // Transform Step: Mime Types
         transformMimeTypes(locale);

         // Transform Step: Postcode By State
         transformPostCodeByState(locale);

         removeAvatarUri(locale);

         ensureAllArraysAreStrings(locale);

         specializeLocale(locale, localeCode);

         var destName = localeCode + ".locale.json";
         log2(destName);
         var bogusLocale = {};

         var extendPath = path.resolve(dataExtendFolder, destName);
         if (fs.existsSync(extendPath)) {
            var extendData = JSON.parse(fs.readFileSync(extendPath, 'utf8'));

            // By default, _.merge replaces items in arrays. IE:
            // _.merge([1,2,3,4], [9,9]) = [9,9,3,4], in our case
            // data extend locale files should replace the full contents
            // of the array, not replace items.
            // https://lodash.com/docs/4.17.10#mergeWith
            var replacer = (objValue, srcValue) => {
               if( _.isArray(objValue) ) {
                  return objValue = srcValue;
               }
            };
            bogusLocale = l.mergeWith(locale, extendData, replacer);
         } else {
            bogusLocale = locale;
         }

         var vinyl = new Vinyl({
            path: './' + destName,
            contents: Buffer.from(JSON.stringify(bogusLocale, null, 2))
         });
         return vinyl;
      }))
      .pipe(print())
      .pipe(lec({ eolc: "CRLF" }))
      .pipe(gulp.dest(dataFolder));
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

function removeAvatarUri(obj){
   if(obj.internet && obj.internet.avatar_uri)
   {
      log("Removing internet.avatar_uri");
      delete obj.internet.avatar_uri;
   }
}

function transformPostCodeByState(obj) {
   if (obj.address && obj.address.postcode_by_state)
      delete obj.address.postcode_by_state;
}

function ensureAllArraysAreStrings(obj) {
   var nodes = jp.nodes(obj, "$..*[*]");
   for (var i = 0; i < nodes.length; i++) {
      var item = nodes[i].value;
      var path = nodes[i].path;
      if (l.isNumber(item)) {
         var pathExpr = jp.stringify(path);
         log(`Replacing number found: ${item} at ${pathExpr} with string.`);
         jp.value(obj, pathExpr, item.toString());
      }
   }
}

function transformCurrency(obj) {
   var currencies = l.get(obj, "finance.currency");
   if (!currencies) return;
   log("Normalizing finance.currency...");
   var arr = l.keys(currencies).map(function (key) {
      var name = key;
      var code = currencies[key]["code"];
      var symbol = currencies[key]["symbol"];
      return { name: name, code: code, symbol: symbol }
   });

   obj["finance"]["currency"] = arr;
}
function transformMimeTypes(obj) {
   var mimes = l.get(obj, "system.mimeTypes");
   if (!mimes) return;
   log("Normalizing system.mimeTypes...");
   var arr = l.keys(mimes).map(function (key) {
      var mime = key;
      var source = mimes[key]["source"];
      var compressible = mimes[key]["compressible"];
      var extensions = mimes[key]["extensions"];
      return { mime: mime, source: source, compressible: compressible, extensions: extensions }
   });

   obj["system"]["mimeTypes"] = arr;
}

function specializeLocale(locale, localeCode) {
   
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