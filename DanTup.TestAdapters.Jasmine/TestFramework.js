// Args:
// 0 = node
// 1 = this file
// 2 = path to Jasmine
// 3 = test file (.jstests)
// 4 = "list" if we should only list (not execute) tests

// Include stuff
jasmine = require(process.argv[2]);
xmlReporter = require('./XmlReporter.js')

// WHAT THE? I DON'T EVEN KNOW WHAT THIS MEANS
jasmine = jasmine.core(jasmine);

// Setup
var env = jasmine.getEnv();
env.specFilter = function () { return process.argv.length !== 5 || process.argv[4] !== 'list'; } // Flag tests not to match the filter if we're listing
env.addReporter(new xmlReporter()); // Set up the console logger

// Create some global functions to avoid putting jasmine.getEnv() everywhere
describe = env.describe;
it = env.it;
expect = env.expect;


// Include tests file
require(process.argv[3]);


// Kick off execution
env.execute();
