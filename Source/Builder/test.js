var faker = require('../fakerjs');
var fs = require('fs');
var _ = require('lodash');
var getSlug = require('../speakingurl')

var val = getSlug('Schöner Titel läßt grüßen!? Bel été !');

console.log(val);
