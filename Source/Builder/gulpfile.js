var gulp = require("gulp");
var $ = require("gulp-load-plugins")({ lazy: true, rename: { 'gulp-cr-lf-replace': 'crlf' } });
var _ = require("underscore");

var es = require("event-stream");

var localeFolders = gulp.src(["../fakerjs/lib/locales/*"]);

var dataFolder = "../Bogus/data";

gulp.task("build.locales", function () {

    return localeFolders
        .pipe($.plumber())
        .pipe($.map(function(file) {
            var localeCode = file.relative;
            var localeIndex = file.path + "/index.js";
            var locale = require(localeIndex);

            var bogusLocale = {};
            bogusLocale[localeCode] = locale;

            var destName = localeCode + ".locale.json";

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


//Helper Methods
function log(msg) {
  $.util.log($.util.colors.bgCyan(msg));
};