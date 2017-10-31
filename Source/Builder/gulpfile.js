var gulp = require("gulp");
var jp = require('jsonpath');
var $ = require("gulp-load-plugins")({ lazy: true, rename: { 'gulp-cr-lf-replace': 'crlf' } });
var _ = require("underscore");
var l = require("lodash");

var path = require("path");
var fs = require("fs");
var BSON = require("bson");

var es = require("event-stream");

var localeFolders = gulp.src(["../fakerjs/lib/locales/*"]);

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

         var destName = localeCode + ".locale.json";
         log2(destName);
         var bogusLocale = {};

         var extendPath = path.resolve(dataExtendFolder, destName);
         if (fs.existsSync(extendPath)) {
            var extendData = JSON.parse(fs.readFileSync(extendPath, 'utf8'));
            bogusLocale = l.merge(locale, extendData);
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

gulp.task("convert.locales.bson", ["import.locales.json"], function () {
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

gulp.task('compress-data', ["convert.locales.bson"],
    $.shell.task('build.cmd compress-data', {cwd:'../../'}) 
);

gulp.task('import.locales', ["compress-data"], function(cb){
      
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


//Helper Methods
function log(msg) {
   $.util.log($.util.colors.bgCyan(msg));
};
function log2(msg) {
   $.util.log($.util.colors.green(msg));
}