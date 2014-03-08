using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DanTup.TestAdapters.Jasmine
{
	// TODO: Ditch some of the cruft in TestFramework.js and use node_suite.js
	// See: http://stackoverflow.com/questions/22127452/whats-the-correct-way-to-use-jasmine-from-node/22128474#22128474
	public class JasmineExternalTestExecutor : ExternalCommandTestExecutor
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
