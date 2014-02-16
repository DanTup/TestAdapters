using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace DanTup.TestAdapters
{
	public abstract class TestDiscoverer : ITestDiscoverer
	{
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
			var path = Path.Combine(directory, test.CodeFilePath.StartsWith(".\\") ? test.CodeFilePath.Substring(2) : test.CodeFilePath);

			return new TestCase(test.Name, ExecutorUri, source)
			{
				DisplayName = test.DisplayName,
				CodeFilePath = path,
				LineNumber = test.LineNumber
			};
		}
	}
}
