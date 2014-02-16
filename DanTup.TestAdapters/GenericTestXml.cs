using System;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace DanTup.TestAdapters
{
	[XmlRoot("Tests")]
	[Serializable]
	public class GenericTests
	{
		[XmlElement("Test")]
		public GenericTest[] Tests { get; set; }
	}

	[Serializable]
	public class GenericTest
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string CodeFilePath { get; set; }
		public int LineNumber { get; set; }

		public TestOutcome Outcome { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorStackTrace { get; set; }
	}
}
