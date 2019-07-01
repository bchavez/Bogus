var faker = require('../fakerjs');
var fs = require('fs');
var _ = require('lodash');
var getSlug = require('../speakingurl')

var val = getSlug('Foo & Bar ♥ Foo < Bar', {symbols: false});

console.log(val);


