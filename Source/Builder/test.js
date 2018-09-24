var faker = require('../fakerjs');

faker.setLocale("ar");
var val = faker.address.city();

console.log(val);