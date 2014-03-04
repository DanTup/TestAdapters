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

// Hijack STDOUT for test run, and put back afterwards
// This allows us to get test output (TODO), and also stop tests interfering with our output XML
var oldWrite = process.stdout.write;
process.stdout.write = function () { }
var restoreOutput = function () {
	process.stdout.write = oldWrite;
}

// Setup
var env = jasmine.getEnv();
env.addReporter(new xmlReporter(restoreOutput)); // Set up the console logger

// Create some global functions to avoid putting jasmine.getEnv() everywhere
describe = env.describe;
xdescribe = env.xdescribe;
beforeEach = env.beforeEach
afterEach = env.afterEach;
it = env.it;
xit = env.xit;
expect = env.expect;
spyOn = env.spyOn;

// SUPERBODGE (See Issue #3)
// Unti we can find a good way of getting locations for the tests, when asked for a list, we will
// force them all to fail by replacing the body of the function with one tat immediately throws
// an error that capture the stack from the real location
if (process.argv.length === 5 && process.argv[4] === 'list') {
	beforeEach = function () { };
	afterEach = function () { };
	spyOn = function () { };
	it = function (description, fn) {
		// Capture the stack for the call to it()
		var error = new Error('TEST LISTING; DUMMY ERROR');

		// Set up a dummy it() that just throws the error we premade
		env.it(description, function () {
			throw error;
		});
	}
}


// Include tests file
require(process.argv[3]);

// Kick off execution
env.execute();

// Tell Node to exit, otherwise it'll hang around if there are any background threads (Issue #8)
// ... but it's not to simply, because writing to stdout is async, so we need to flush
// ... but, flush is also Async (OMGWTF!), so we need to throw in a callback to exit when it drains
process.stdout.once('drain', function() {
	process.exit()
})
