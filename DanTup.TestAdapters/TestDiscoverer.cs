using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace DanTup.TestAdapters
{
	public abstract class TestDiscoverer : ITestDiscoverer
	{
		/// <summary>
		/// Location that the extension is installed; used for locating resources that were included (eg. lua.exe).
		/// </summary>
		public abstract string ExtensionFolder { get; }

		protected abstract ExternalTestExecutor ExternalTestExecutor { get; }
		protected abstract Uri ExecutorUri { get; }

		public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
		{
			foreach (var source in sources)
				DiscoverTests(source, discoveryContext, logger, discoverySink);
		}

		public void DiscoverTests(string source, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
		{
			foreach (var test in ExternalTestExecutor.GetTestCases(source, s => logger.SendMessage(TestMessageLevel.Error, s)).Select(c => CreateTestCase(source, c)))
				discoverySink.SendTestCase(test);
		}

		private TestCase CreateTestCase(string source, GenericTest test)
		{
			var directory = Path.GetDirectoryName(source);
			// VS will open the file, but not jump to the line number if the path contais "\.\", so make sure we handle this
			var path = test.CodeFilePath != null ? Path.Combine(directory, test.CodeFilePath.StartsWith(".\\") ? test.CodeFilePath.Substring(2) : test.CodeFilePath) : null;
			var line = test.LineNumber;

			// SUPERBODGE (See Issue #3)
			// If we don't have the location of the test, but we have a stack, parse it out
			// This is temporary until we can find a better way to get filename/line numbers via Jasmine
			if (path == null && !string.IsNullOrWhiteSpace(test.ErrorStackTrace))
				TryParseLocationFromStack(test, out path, out line);

			return new TestCase(test.Name, ExecutorUri, source)
			{
				DisplayName = test.DisplayName,
				CodeFilePath = path,
				LineNumber = line
			};
		}

		private void TryParseLocationFromStack(GenericTest test, out string path, out int line)
		{
			// First line is the error message
			// Then there's one line of our framework; but just to be a bit more future-proof, exclude any that look like extension
			var possibleLines = test.ErrorStackTrace.Split('\n').Skip(1).Where(l => !l.ToLower().Contains(ExtensionFolder.ToLower()));

			foreach (var matchingLine in possibleLines)
			{
				var match = Regex.Match(matchingLine, @"\((.+):(\d+):\d+\)", RegexOptions.Compiled);

				if (match.Groups.Count >= 3)
				{
					path = match.Groups[1].Value.Replace("/", @"\");
					line = int.Parse(match.Groups[2].Value);
					return;
				}
			}

			path = null;
			line = 0;
		}
	}
}
