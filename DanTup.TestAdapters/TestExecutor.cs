using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace DanTup.TestAdapters
{
	public abstract class TestExecutor : ITestExecutor
	{
		protected abstract ExternalTestExecutor ExternalTestExecutor { get; }
		protected abstract Uri ExecutorUri { get; }

		public void Cancel()
		{
			throw new NotImplementedException();
		}

		public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
		{
			foreach (var source in sources)
				RunTests(source, runContext, frameworkHandle);
		}

		private void RunTests(string source, IRunContext runContext, IFrameworkHandle frameworkHandle)
		{
			foreach (var result in ExternalTestExecutor.GetTestResults(source, null).Select(c => CreateTestResult(source, c)))
			{
				frameworkHandle.RecordStart(result.TestCase);
				frameworkHandle.RecordResult(result);
				frameworkHandle.RecordEnd(result.TestCase, result.Outcome);
			}
		}

		public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
		{
			// HACK: For now, just run them all - we can fix this later :)
			RunTests(tests.Select(t => t.Source).Distinct(), runContext, frameworkHandle);
		}

		private TestResult CreateTestResult(string source, GenericTest test)
		{
			var directory = Path.GetDirectoryName(source);
			// VS will open the file, but not jump to the line number if the path contais "\.\", so make sure we handle this
			var path = test.CodeFilePath != null ? Path.Combine(directory, test.CodeFilePath.StartsWith(".\\") ? test.CodeFilePath.Substring(2) : test.CodeFilePath) : null;

			var testCase = new TestCase(test.Name, ExecutorUri, source)
			{
				DisplayName = test.DisplayName,
				CodeFilePath = path,
				LineNumber = test.LineNumber
			};

			return new TestResult(testCase)
			{
				Outcome = test.Outcome,
				ErrorMessage = test.ErrorMessage,
				ErrorStackTrace = test.ErrorStackTrace
			};
		}
	}
}
