using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DanTup.TestAdapters.Xml
{
	public class XmlExternalTestExecutor : ExternalTestExecutor
	{
		static readonly string extensionFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public override string ExtensionFolder { get { return extensionFolder; } }

		/// <summary>
		/// Reads the results XML file directly.
		/// </summary>
		public override IEnumerable<GenericTest> GetTestCases(string source, Action<string> logger)
		{
			return ParseTestOutput(File.ReadAllText(source));
		}

		/// <summary>
		/// Reads the results XML file directly.
		/// </summary>
		public override IEnumerable<GenericTest> GetTestResults(string source, Action<string> logger)
		{
			return ParseTestOutput(File.ReadAllText(source));
		}
	}
}
