using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DanTup.TestAdapters
{
	/// <summary>
	/// Base class for external executors of tests that return results in the desired XML format.
	/// </summary>
	public abstract class ExternalTestExecutor
	{
		/// <summary>
		/// Location that the extension is installed; used for locating resources that were included (eg. lua.exe).
		/// </summary>
		public abstract string ExtensionFolder { get; }

		/// <summary>
		/// Gets the tests cases via the external command.
		/// </summary>
		public IEnumerable<GenericTest> GetTestCases(string source, Action<string> logger)
		{
			return ParseTestOutput(ExecuteTestCommand(source, logger, "list", "listing tests"));
		}

		/// <summary>
		/// Executes the tests and gets the results via the external command.
		/// </summary>
		public IEnumerable<GenericTest> GetTestResults(string source, Action<string> logger)
		{
			return ParseTestOutput(ExecuteTestCommand(source, logger, "", "executing tests"), removeExtensionDirectoryFromSackTraces: true);
		}

		/// <summary>
		/// Parse the XML returned by the external command into our generic test class.
		/// </summary>
		private IEnumerable<GenericTest> ParseTestOutput(string testXml, bool removeExtensionDirectoryFromSackTraces = false)
		{
			var tests = (GenericTests)new XmlSerializer(typeof(GenericTests)).Deserialize(new StringReader(testXml));

			if (removeExtensionDirectoryFromSackTraces)
				tests.Tests = CleanupResults(tests.Tests).ToArray();

			return tests.Tests ?? Enumerable.Empty<GenericTest>();
		}

		private IEnumerable<GenericTest> CleanupResults(IEnumerable<GenericTest> tests)
		{
			// Don't tell anyone we're mutating data here, won't go down well...

			foreach (var t in tests)
			{
				// Strip VS extension folder out of stack traces, to keep them less noisy
				if (!string.IsNullOrWhiteSpace(t.ErrorStackTrace))
				{
					t.ErrorStackTrace = t.ErrorStackTrace
						.Replace(ExtensionFolder + @"\", "")
						.Replace(ExtensionFolder.ToUpper() + @"\", "")
						.Replace(ExtensionFolder.ToLower() + @"\", ""); // HACK: Don't get me started on VS's random all-lowercasing or all-uppercasing of paths :(
				}
			}

			return tests;
		}

		/// <summary>
		/// Execute the external command and wait for the output.
		/// </summary>
		private string ExecuteTestCommand(string source, Action<string> logger, string args, string action)
		{
			// TODO: Currently we execute all tests and get the outpuit in one go; we should change this to handle the XML nodes as they come in.
			// TODO: Check how nicely we're handling crashes.
			using (var proc = new Process { StartInfo = CreateProcessStartInfo(source, args) })
			{
				proc.Start();

				var output = proc.StandardOutput.ReadToEnd();
				var error = proc.StandardError.ReadToEnd();
				proc.WaitForExit();

				if (logger != null && !string.IsNullOrEmpty(error))
					logger(string.Format("Error {0}: {1}", action, error));

				return output;
			}
		}

		protected abstract ProcessStartInfo CreateProcessStartInfo(string source, string args);
	}
}
