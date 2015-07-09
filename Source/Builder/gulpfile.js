var gulp = require("gulp");
var $ = require("gulp-load-plugins")({ lazy: true });
var _ = require("underscore");

var es = require("event-stream");

var locales = require("../fakerjs/lib/locales.js");

var dataFolder = "../Bogus/data";

gulp.task("build.locales", function() {

  var localeCodes = _.keys(locales);

  var files = _.map(localeCodes, function(code) {

    var destName = code + ".locale.json";

    var thisLocale = {};
    thisLocale[code] = locales[code];

    var localeFile = new $.util.File({
      path: './' + destName,
      contents: new Buffer(JSON.stringify(thisLocale, null, 2))
    });

    return localeFile;
  });

  return es.readArray(files)
    .pipe($.print())
    .pipe(gulp.dest(dataFolder));
});


//Helper Methods
function log(msg) {
  $.util.log($.util.colors.bgCyan(msg));
};