using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace DanTup.TestAdapters.Xml
{
	[FileExtension(".testxml")]
	[DefaultExecutorUri(XmlTestExecutor.TestExecutorUriString)]
	public class XmlTestDiscoverer : TestDiscoverer
	{
		static readonly string extensionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public override string ExtensionFolder { get { return extensionFolder; } }

		readonly XmlExternalTestExecutor executor = new XmlExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }

		protected override Uri ExecutorUri { get { return XmlTestExecutor.TestExecutorUri; } }
	}
}
