using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using Microsoft.VisualStudio.TestWindow.Extensibility.Model;

namespace DanTup.TestAdapters
{
	/// <summary>
	/// Basic TestContainer that does the minimum possible for VS to be happy.
	/// </summary>
	public class TestContainer : ITestContainer
	{
		public TestContainer(ITestContainerDiscoverer discoverer, string source)
		{
			this.Discoverer = discoverer;
			this.Source = source;
			this.CreatedTime = DateTime.Now;
		}

		public DateTime CreatedTime { get; private set; }

		public int CompareTo(ITestContainer other)
		{
			var otherContainer = other as TestContainer;
			if (otherContainer == null)
				return -1;

			if (this.Source != otherContainer.Source)
				return this.Source.CompareTo(otherContainer.Source);

			return this.CreatedTime.CompareTo(otherContainer.CreatedTime);
		}

		public IEnumerable<Guid> DebugEngines
		{
			get { return Enumerable.Empty<Guid>(); }
		}

		public IDeploymentData DeployAppContainer()
		{
			return null;
		}

		public ITestContainerDiscoverer Discoverer { get; private set; }

		public bool IsAppContainerTestContainer
		{
			get { return false; }
		}

		public ITestContainer Snapshot()
		{
			return this;
		}

		public string Source { get; private set; }

		public FrameworkVersion TargetFramework
		{
			get { return FrameworkVersion.None; }
		}

		public Architecture TargetPlatform
		{
			get { return Architecture.AnyCPU; }
		}
	}
}
