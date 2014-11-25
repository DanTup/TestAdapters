using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System;

namespace DanTup.TestAdapters.Lua
{
	public class LuaExternalTestExecutor : ExternalCommandTestExecutor
	{
		static readonly string extensionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public override string ExtensionFolder { get { return extensionFolder; } }

		static readonly string luaExecutable = Path.Combine(extensionFolder, "luajit.exe");
		static readonly string testFrameworkFile = Path.Combine(extensionFolder, "TestFramework.lua");

        public override System.Collections.Generic.IEnumerable<GenericTest> GetTestCases(string source, System.Action<string> logger)
        {
            return source.EndsWith("tests.lua") ? base.GetTestCases(source, logger) : Enumerable.Empty<GenericTest>();
        }

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
