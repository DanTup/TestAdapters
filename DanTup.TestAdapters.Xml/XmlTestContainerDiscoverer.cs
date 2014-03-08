using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace DanTup.TestAdapters.Xml
{
	[Export(typeof(ITestContainerDiscoverer))]
	public class XmlTestContainerDiscoverer : TestContainerDiscoverer
	{
		protected override string[] TestContainerFileExtensions { get { return new[] { ".testxml" }; } }
		protected override string[] WatchedFilePatterns { get { return new[] { "*.testxml" }; } }

		[ImportingConstructor]
		public XmlTestContainerDiscoverer([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override Uri ExecutorUri
		{
			get { return XmlTestExecutor.TestExecutorUri; }
		}
	}
}
