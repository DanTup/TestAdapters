using System.Diagnostics;
using System.IO;

namespace DanTup.TestAdapters.Lua
{
	public class LuaExternalTestExecutor : ExternalTestExecutor
	{
		static readonly string luaExecutable = Path.Combine(extensionFolder, "lua52.exe");
		static readonly string testFrameworkFile = Path.Combine(extensionFolder, "TestFramework.lua");

		protected override ProcessStartInfo CreateProcessStartInfo(string source, string args)
		{
			args = string.Format("\"{0}\" \"{1}\" {2}", testFrameworkFile.Replace("\"", "\\\""), source.Replace("\"", "\\\""), args);

			return new ProcessStartInfo(luaExecutable, args)
			{
				WorkingDirectory = Path.GetDirectoryName(source),
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
		}
	}
}
