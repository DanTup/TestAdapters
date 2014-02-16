using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace DanTup.TestAdapters.Lua
{
	[Export(typeof(ITestContainerDiscoverer))]
	public class LuaTestContainerDiscoverer : TestContainerDiscoverer
	{
		protected override string TestContainerFileExtension { get { return ".luatests"; } }
		protected override string[] WatchedFilePatterns { get { return new[] { "*.luatests", "*.lua" }; } }

		[ImportingConstructor]
		public LuaTestContainerDiscoverer([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override Uri ExecutorUri
		{
			get { return LuaTestExecutor.TestExecutorUri; }
		}
	}
}
