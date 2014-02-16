Lua Test Adapter and Framework
=========

[![Lua Test Adapter and Framework for Visual Studio](http://img.youtube.com/vi/fW3L3LTMdBw/0.jpg)](http://www.youtube.com/watch?v=fW3L3LTMdBw)

Features
===

- Automatically discovers test functions that begin with "test_" from .luatests files
- .luatest files can include other .lua files using require or dofile
- Automatically update test list when any .lua or .luatests file is updated
- Automatically executes tests when any .lua or .luatests file is if "Run tests after build" is selected in the Visual Studio Test Explorer
- Executes testframework_setup and testframework_teardown functions (if found) before/after each test

Installation
===

1. Install from [Visual Studio Gallery](http://visualstudiogallery.msdn.microsoft.com/8a046271-217f-48b6-8293-2b8447081695)

Usage
===
1. Create a .luatests file in any Visual Studio project
2. Include any additional .lua files using require or dofile
3. Start any test fuctions with test_ and use assert() or throw errors for failures
4. Create a testframework_setup or testframework_teardown function if you have code that must be run before/after each test

Known Issues / Limitations
===
1. [Issue #1](/../../issues/1) Currently all tests will always be executed, even if you select a subset of tests
2. [Issue #2](/../../issues/2) Currently all tests must finish executing before results are displayed

Feedback
===
Please send your feedback/issues/feature requests! :-)

- GitHub Issues: [TestAdapters/issues](https://github.com/DanTup/TestAdapters/issues)
- Twitter: [@DanTup](https://twitter.com/DanTup)
- Google+: [Danny Tuppeny](http://profile.dantup.com/)
- Email: [danny+testadapters@tuppeny.com](mailto:danny+testadapters@tuppeny.com)
