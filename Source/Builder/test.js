var faker = require('../fakerjs');

faker.setLocale("az");
var val = faker.address.state();

console.log(val);