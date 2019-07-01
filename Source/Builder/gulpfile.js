var gulp = require("gulp");
var jp = require('jsonpath');
var $ = require("gulp-load-plugins")({ lazy: true, rename: { 'gulp-cr-lf-replace': 'crlf' } });
var _ = require("underscore");
var l = require("lodash");

var path = require("path");
var fs = require("fs");
var BSON = require("bson");

var es = require("event-stream");

var localeFolders = gulp.src(
   [
      "../fakerjs/lib/locales/*",
      "!../fakerjs/lib/locales/ar" // 2018.09.23 - Exclude this locale, has problems upstream.
                                   // https://github.com/Marak/faker.js/pull/505/files#r219737439
   ]);

var dataFolder = "../Bogus/data";
var dataExtendFolder = "../Bogus/data_extend";

gulp.task("import.locales.json", function () {

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

         var vinyl = new $.util.File({
            path: './' + destName,
            contents: new Buffer(JSON.stringify(bogusLocale, null, 2))
         });
         return vinyl;
      }))
      .pipe($.print())
      .pipe($.crlf({ changeCode: "CR+LF" }))
      .pipe(gulp.dest(dataFolder));
});

gulp.task("import.locales", ["import.locales.json"], function () {
   return gulp.src(`${dataFolder}/*.locale.json`)
      .pipe($.plumber())
      .pipe($.map(function (file) {
         var json = JSON.parse(file.contents.toString());

         var destName = `${path.basename(file.relative, ".json")}.bson`;

         var b = new BSON();
         var data = b.serialize(json, { checkKeys: true });

         var vinyl = new $.util.File({
            path: './' + destName,
            contents: new Buffer(data)
         });
         return vinyl;
      }))
      .pipe($.print())
      .pipe(gulp.dest(dataFolder));
});

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
   $.util.log($.util.colors.bgCyan(msg));
};
function log2(msg) {
   $.util.log($.util.colors.green(msg));
}


gulp.task("import.speakingurl", () => {
   //strip out the module scoping of the library
   var src = fs.readFileSync('../speakingurl/lib/speakingurl.js', 'utf8');
   var lines = src.split('\n');
   var moduleEnd = _.findIndex(lines, i => i.includes("typeof module") )
   var fixedSource = lines.splice(2, moduleEnd - 2).join('\n');

   //evaluate the whole module without function scoping
   //exposing intenral variables that we can dump.
   eval(fixedSource)

   //varables we get after eval():
   // charMap, lookAheadCharArray, diatricMap, langCharMap, symbolMap, uricChars, uricNoSlashChars, markChars
   //So... let's convert everything to C#.

   //Some helpful render templates.
   function renderKv(key, val){
      if( val === '"') val = '""'
      return `         {@"${key}", @"${val}"},`
   }
   function renderChar(v){
      if(v === "'") v = "\\'";
      return ` '${v}'`;
   }

   function renderNestedDict(k, obj){

      var nested = _.map(obj, (subvalue, subkey)=>{
                  return '    '+renderKv(subkey, subvalue);
         }).join('\r\n');
                           
      var template = `         { @"${k}",  new Dictionary<string,string>{
${nested}}
        },`;
        return template;
   }

   //convert to C# dictionary entries.
   var charMapItems = _.map(charMap, (v,k)=>{
                           return renderKv(k, v);
                        });
   var lookAheadItems = _.map(lookAheadCharArray, v =>{
                           return renderChar(v);
                        });

   var diatricMapItems = _.map(diatricMap, (v,k)=>{
                           return renderKv(k, v)
                        });
   var langCharMapItems = _.map(langCharMap, (v,k)=>{
                           return renderNestedDict(k, v)
                        });

   var symbolMapItems = _.map(symbolMap, (v,k)=>{
      return renderNestedDict(k, v);
   });

   var uricCharItems = _.map(uricChars, v =>{
      return renderChar(v);
   });

   var uricNoSlashCharsItems = _.map(uricNoSlashChars, v => {
      return renderChar(v);
   });

   var markCharsItems = _.map(markChars, v =>{
      return renderChar(v);
   });


   var template = `
// AUTO GENERATED FILE. DO NOT MODIFY.
// SEE Builder/gulpfile.js import.speakingurl task.
using System.Collections.Generic;
namespace Bogus
{
   public static partial class Slugger
   {
      public static char[] LookAheadArray = new char[]{${lookAheadItems.join(',')} };
      public static char[] UricChars = new char[]{${uricCharItems.join(',')} };
      public static char[] UricNoShashChars = new char[]{${uricNoSlashCharsItems.join(',')} };
      public static char[] MarkChars = new char[]{${markCharsItems.join(',')} };

      public static Dictionary<string,string> CharMap = new Dictionary<string,string>()
      {
${charMapItems.join('\r\n')}
      };

      public static Dictionary<string,string> DiatricMap = new Dictionary<string,string>()
      {
${diatricMapItems.join('\r\n')}
      };

      public static Dictionary<string, Dictionary<string, string>> LangCharMap = new Dictionary<string, Dictionary<string, string>>()
      {
${langCharMapItems.join('\r\n')}
      };

      public static Dictionary<string, Dictionary<string, string>> SymbolMap = new Dictionary<string, Dictionary<string, string>>()
      {
${symbolMapItems.join('\r\n')}
      };
   }
}
`

   fs.writeFileSync('../Bogus/Slugger.Generated.cs', template);

});


gulp.task("import.transliterate", () => {

   //strip out the module scoping of the library
   var src = fs.readFileSync('../speakingurl/lib/speakingurl.js', 'utf8');
   var lines = src.split('\n');
   var moduleEnd = _.findIndex(lines, i => i.includes("typeof module") )
   var fixedSource = lines.splice(2, moduleEnd - 2).join('\n');

   //evaluate the whole module without function scoping
   //exposing intenral variables that we can dump.
   eval(fixedSource);
   // a, ae
//    function renderTrieItem(input, idx, val){
//       var pad = l.padStart('',12*(idx+1), ' ');
//       if( val === '"') val = '""'
//       if( idx == input.length - 1 ){
//          return `
// ${pad}{ @"${input[idx]}", new Trie(@"${input[idx]}")
// ${pad}            {
// ${pad}               Value = @"${val}"
// ${pad}            }
// ${pad}},`
//       }
//       else {
         
//          var nestedTrie = renderTrieItem(input, idx + 1, val)
//          return `
// ${pad}{ @"${input[idx]}", new Trie(@"${input[idx]}")
// ${pad}            {
// ${pad}               Map = new Dictionary<string, Trie>
// ${pad}               {
// ${pad}                           ${nestedTrie}
// ${pad}               }
// ${pad}            }
// ${pad}},`

//       }
//    }
   // function renderRoot(dict){
   //    var entries = _.map(dict, (v,k)=>{
   //          return renderTrieItem(k, 0, v);
   //    });

   //    entries = entries.join('\r\n');

   //    return `new Trie{
   //       Map = new Dictionary<string, Trie>{
   //          ${entries}
   //       } 
   //    };`
   // }

   // var rootSample = [
   //    {key: 'a',
   //     val: 'aa',
   //     map: [
   //             {key:'b',
   //              val: 'ab',
   //              map: [
   //                     {key:'z',
   //                      val:'abz'}
   //                ]
   //             },
   //             {key:'c',
   //              val: 'ac'}
   //          ]
   //    }
   // ]


   // function insertTrie(node, key, val){
   //    for(var i = 0; i < key.length; i++){
   //       var ch = key[i];
   //       if( node[ch] === undefined ){
   //          node[ch] = {};
   //       }
   //       node = node[ch];
   //    }
   //    node.val = val;
   // }

   // function buildTrie(obj){
   //    var root = {};
   //    var kvs = [];
   //    _.map( obj, (v,k) => {
   //         return kvs.push({key: k, val: v});
   //    });

   //    for(var i = 0; i < kvs.length; i++){
   //       var key = kvs[i].key;
   //       var val = kvs[i].val;
         
   //       insertTrie(root, key, val);
   //    }

   //    return root;
   // }


   //convert to C# dictionary entries.
   //var charMapTrie = renderRoot(charMap);
   //var charMapTrie = buildTrie(charMap)

   function renderInsert(obj){
      var inserts = [];
      _.map( obj, (v,k) => {
         if( v === '"') v = '""';
         return inserts.push(`            Trie.Insert(trie, @"${k}", @"${v}");`);
      });
      return inserts;
   }
   function renderMdInsert(obj){
      var inserts = [];
      _.map( obj, (v,k) => {
         _.map(v, (v2, k2)=>{
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
      [EditorBrowsable(EditorBrowsableState.Never)]
      public static partial class TransliterateData
      {   
         public static Trie BuildCharMap(Trie trie)
         {
${charMapInserts.join('\r\n')}
            return trie;
         }

         public static Trie BuildDiatricMap(Trie trie)
         {
${diatricMapInserts.join('\r\n')}
            return trie;
         }

         public static MultiDictionary<string,string,string> BuildLangCharMap(MultiDictionary<string,string,string> md)
         {
${langCharInserts.join('\r\n')}
            return md;
         }

         public static MultiDictionary<string,string,string> BuildSymbolMap(MultiDictionary<string,string,string> md)
         {
${symbolInserts.join('\r\n')}
            return md;
         }
      }
   }
   `
   
   
   fs.writeFileSync('../Bogus/TransliterateData.Generated.cs', template);


});