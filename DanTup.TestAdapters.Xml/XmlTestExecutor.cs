using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace DanTup.TestAdapters.Xml
{
	[ExtensionUri(XmlTestExecutor.TestExecutorUriString)]
	public class XmlTestExecutor : TestExecutor
	{
		public const string TestExecutorUriString = "executor://dantup.testadapters.xml/v1";
		public static Uri TestExecutorUri = new Uri(TestExecutorUriString);

		readonly ExternalTestExecutor executor = new XmlExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }
		protected override Uri ExecutorUri { get { return TestExecutorUri; } }
	}
}
