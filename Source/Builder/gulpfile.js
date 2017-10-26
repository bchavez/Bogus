var gulp = require("gulp");
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

gulp.task("build.locale.json", function () {

    return localeFolders
        .pipe($.plumber())
        .pipe($.map(function(file) {
            var localeCode = file.relative;
            var localeIndex = file.path + "/index.js";
            var locale = require(localeIndex);

            var destName = localeCode + ".locale.json";
            var bogusLocale = {};

            var extendPath = path.resolve(dataExtendFolder, destName);
            if( fs.existsSync(extendPath) ) {
                var extendData = JSON.parse(fs.readFileSync(extendPath, 'utf8'));
                bogusLocale[localeCode] = l.merge(locale, extendData);
            } else {
                bogusLocale[localeCode] = locale;                
            }

            var vinyl = new $.util.File({
                path: './' + destName,
                contents: new Buffer(JSON.stringify(bogusLocale, null, 2))
            });
            return vinyl;
        }))
        .pipe($.print())
        .pipe($.crlf({changeCode: "CR+LF"}))
        .pipe(gulp.dest(dataFolder));
});

gulp.task("build.locales", [], function(){
    return gulp.src(`${dataFolder}/*.locale.json`)
    .pipe($.plumber())
    .pipe($.map(function(file) {
        var json = JSON.parse(file.contents.toString());

        var destName = `${file.relative}.bin`;

        var b = new BSON();
        var data = b.serialize(json, {checkKeys: false});

        var vinyl = new $.util.File({
            path: './' + destName,
            contents: new Buffer(data)
        });
        return vinyl;
    }))
    .pipe($.print())
    .pipe(gulp.dest(dataFolder));
});

//Helper Methods
function log(msg) {
  $.util.log($.util.colors.bgCyan(msg));
};