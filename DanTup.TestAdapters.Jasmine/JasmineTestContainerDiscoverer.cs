using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace DanTup.TestAdapters.Jasmine
{
	[Export(typeof(ITestContainerDiscoverer))]
	public class JasmineTestContainerDiscoverer : TestContainerDiscoverer
	{
		protected override string TestContainerFileExtension { get { return ".jstests"; } }
		protected override string[] WatchedFilePatterns { get { return new[] { "*.jstests", "*.js" }; } }

		[ImportingConstructor]
		public JasmineTestContainerDiscoverer([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override Uri ExecutorUri
		{
			get { return JasmineTestExecutor.TestExecutorUri; }
		}
	}
}
