using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DanTup.TestAdapters.Xml
{
	public class XmlExternalTestExecutor : ExternalTestExecutor
	{
		static readonly string extensionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public override string ExtensionFolder { get { return extensionFolder; } }

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
