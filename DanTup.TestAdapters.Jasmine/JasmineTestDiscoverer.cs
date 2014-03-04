using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace DanTup.TestAdapters.Jasmine
{
	[FileExtension(".jstest")]
	[FileExtension(".jstests")]
	[DefaultExecutorUri(JasmineTestExecutor.TestExecutorUriString)]
	public class JasmineTestDiscoverer : TestDiscoverer
	{
		static readonly string extensionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public override string ExtensionFolder { get { return extensionFolder; } }

		readonly JasmineExternalTestExecutor executor = new JasmineExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }

		protected override Uri ExecutorUri { get { return JasmineTestExecutor.TestExecutorUri; } }
	}
}
