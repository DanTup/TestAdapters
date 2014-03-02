Jasmine Test Adapter
=========

[![Jasmine Test Adapter for Visual Studio](http://img.youtube.com/vi/Gc4xLjUxxOY/0.jpg)](http://www.youtube.com/watch?v=Gc4xLjUxxOY)

Features
===

- Automatically update test list when any .js or .jstests file is updated
- Automatically executes tests when any .js or .jstests file is if "Run tests after build" is selected in the Visual Studio Test Explorer

Installation
===

1. Install from [Visual Studio Gallery](http://visualstudiogallery.msdn.microsoft.com/102979e0-61ba-4c6f-a18c-ca64cc7bd2c6)

Usage
===
1. Create a .jstests file in any Visual Studio project
2. Include any additional .js files using Node's require()
3. Create tests using describe/it/expect; see [Jasmine website](http://jasmine.github.io/) for full details

Known Issues / Limitations
===
1. [Issue #1](/../../issues/1) Currently all tests will always be executed, even if you select a subset of tests
2. [Issue #2](/../../issues/2) Currently all tests must finish executing before results are displayed
3. [Issue #2](/../../issues/2) Currently tests do not display filename/line number or allow quickly jumping to them from the Test Explorer window

Feedback
===
Please send your feedback/issues/feature requests! :-)

- GitHub Issues: [TestAdapters/issues](https://github.com/DanTup/TestAdapters/issues)
- Twitter: [@DanTup](https://twitter.com/DanTup)
- Google+: [Danny Tuppeny](http://profile.dantup.com/)
- Email: [danny+testadapters@tuppeny.com](mailto:danny+testadapters@tuppeny.com)
