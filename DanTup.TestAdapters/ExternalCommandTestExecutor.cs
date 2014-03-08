using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DanTup.TestAdapters
{
	/// <summary>
	/// Base class for external executors of tests that return results in the desired XML format.
	/// </summary>
	public abstract class ExternalCommandTestExecutor : ExternalTestExecutor
	{
		/// <summary>
		/// Gets the tests cases via the external command.
		/// </summary>
		public override IEnumerable<GenericTest> GetTestCases(string source, Action<string> logger)
		{
			return ParseTestOutput(ExecuteTestCommand(source, logger, "list", "listing tests"));
		}

		/// <summary>
		/// Executes the tests and gets the results via the external command.
		/// </summary>
		public override IEnumerable<GenericTest> GetTestResults(string source, Action<string> logger)
		{
			return ParseTestOutput(ExecuteTestCommand(source, logger, "", "executing tests"), removeExtensionDirectoryFromSackTraces: true);
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
