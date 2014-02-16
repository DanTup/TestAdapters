using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace DanTup.TestAdapters.Lua
{
	[FileExtension(".luatests")]
	[DefaultExecutorUri(LuaTestExecutor.TestExecutorUriString)]
	public class LuaTestDiscoverer : TestDiscoverer
	{
		readonly LuaExternalTestExecutor executor = new LuaExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }

		protected override Uri ExecutorUri { get { return LuaTestExecutor.TestExecutorUri; } }
	}
}
