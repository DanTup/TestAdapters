using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace DanTup.TestAdapters.Jasmine
{
	[ExtensionUri(JasmineTestExecutor.TestExecutorUriString)]
	public class JasmineTestExecutor : TestExecutor
	{
		public const string TestExecutorUriString = "executor://dantup.testadapters.jasmine/v1";
		public static Uri TestExecutorUri = new Uri(TestExecutorUriString);

		readonly ExternalTestExecutor executor = new JasmineExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }
		protected override Uri ExecutorUri { get { return TestExecutorUri; } }
	}
}
