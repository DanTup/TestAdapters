using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace DanTup.TestAdapters.Jasmine
{
	[FileExtension(".jstests")]
	[DefaultExecutorUri(JasmineTestExecutor.TestExecutorUriString)]
	public class JasmineTestDiscoverer : TestDiscoverer
	{
		readonly JasmineExternalTestExecutor executor = new JasmineExternalTestExecutor();

		protected override ExternalTestExecutor ExternalTestExecutor { get { return executor; } }

		protected override Uri ExecutorUri { get { return JasmineTestExecutor.TestExecutorUri; } }
	}
}
