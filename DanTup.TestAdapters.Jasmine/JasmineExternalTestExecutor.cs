using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DanTup.TestAdapters.Jasmine
{
	public class JasmineExternalTestExecutor : ExternalTestExecutor
	{
		static readonly string extensionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public override string ExtensionFolder { get { return extensionFolder; } }

		static readonly string nodeExecutable = Path.Combine(extensionFolder, "node.exe");
		static readonly string testFrameworkFile = Path.Combine(extensionFolder, "TestFramework.js");
		static readonly string jasmineFile = Path.Combine(extensionFolder, "jasmine.js");

		protected override ProcessStartInfo CreateProcessStartInfo(string source, string args)
		{
			args = string.Format("\"{0}\" \"{1}\" \"{2}\" {3}", testFrameworkFile.Replace("\"", "\\\""), jasmineFile.Replace("\"", "\\\""), source.Replace("\"", "\\\""), args);

			return new ProcessStartInfo(nodeExecutable, args)
			{
				WorkingDirectory = Path.GetDirectoryName(source),
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
		}
	}
}
