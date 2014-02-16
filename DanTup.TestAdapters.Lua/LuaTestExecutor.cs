using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace DanTup.TestAdapters.Lua
{
	[ExtensionUri(LuaTestExecutor.TestExecutorUriString)]
	public class LuaTestExecutor : TestExecutor
	{
		public const string TestExecutorUriString = "executor://dantup.testadapters.lua/v1";
		public static Uri TestExecutorUri = new Uri(TestExecutorUriString);

		readonly ExternalTestExecutor executor = new LuaExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }
		protected override Uri ExecutorUri { get { return TestExecutorUri; } }
	}
}
