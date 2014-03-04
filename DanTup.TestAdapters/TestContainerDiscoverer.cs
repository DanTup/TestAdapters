using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace DanTup.TestAdapters
{
	/// <summary>
	/// Base TestContainerDiscoverer that scans projects for certain file extensions assumed to be tests and
	/// watches them for changes, notifying VS when the tests may have been updated.
	/// </summary>
	public abstract class TestContainerDiscoverer : ITestContainerDiscoverer, IDisposable, IVsSolutionEvents, IVsHierarchyEvents
	{
		public abstract Uri ExecutorUri { get; }
		protected abstract string[] TestContainerFileExtensions { get; }
		protected abstract string[] WatchedFilePatterns { get; }

		readonly IVsSolution solutionService;
		readonly MultiFileSystemWatcher watchers = new MultiFileSystemWatcher();
		MultiFileSystemWatcher watcher;
		TestContainer[] cachedTestContainers;

		protected TestContainerDiscoverer(IServiceProvider serviceProvider)
		{
			this.solutionService = (IVsSolution)serviceProvider.GetService(typeof(SVsSolution));
			uint pdwCookie;
			this.solutionService.AdviseSolutionEvents(this, out pdwCookie);

			// In order to throttle events, use wire up the real event to TestContainersUpdatedInternal via an Rx Sample()
			Observable
				.FromEventPattern<EventHandler, EventArgs>(x => this.TestContainersUpdatedInternal += x, x => this.TestContainersUpdatedInternal -= x)
				.Sample(TimeSpan.FromMilliseconds(250))
				.Subscribe(e => TestContainersUpdated(this, EventArgs.Empty));
		}

		public IEnumerable<ITestContainer> TestContainers
		{
			get
			{
				if (cachedTestContainers == null)
					UpdateTestContainers();

				return cachedTestContainers;
			}
		}

		/// <summary>
		/// Updates the (cached) list of test containers if it's not already populated.
		/// </summary>
		void UpdateTestContainers()
		{
			lock (this) // HACK: Make all this better
			{
				if (cachedTestContainers != null)
					return;

				cachedTestContainers = solutionService
					.GetProjects()
					.SelectMany(p => p.GetProjectItems())
					.Where(File.Exists)
					.Where(f => this.TestContainerFileExtensions.Any(ext => f.EndsWith(ext)))
					.Select(f => new TestContainer(this, f))
					.ToArray();

				SetupWatchers();
			}
		}

		/// <summary>
		/// Sets up filesystem watches to watch directories for each solution for changes.
		/// </summary>
		private void SetupWatchers()
		{
			if (watcher != null)
				watcher.Dispose();

			watcher = new MultiFileSystemWatcher();
			watcher.FileChanged += TestContainerUpdated;

			// Subscribe to the solution directory, which should pick up most changes
			string solutionDirectory, solutionFile, userOptionsFile;
			if (solutionService.GetSolutionInfo(out solutionDirectory, out solutionFile, out userOptionsFile) == VSConstants.S_OK)
			{
				// Ensure the solution dir ends with \ so we can compare when looking for subfolders
				if (!solutionDirectory.EndsWith("\\"))
					solutionDirectory = solutionDirectory + "\\";

				foreach (var filePattern in this.WatchedFilePatterns)
					watcher.AddWatcher(solutionDirectory, filePattern);

				// Get all directories that had test containers that are not already children of the solution dir (it's already being watched)
				var dirs = cachedTestContainers
					.Select(tc => Path.GetDirectoryName(tc.Source) + "\\")
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.Where(tc => !tc.StartsWith(solutionDirectory, StringComparison.OrdinalIgnoreCase));

				foreach (var dir in dirs)
				{
					foreach (var filePattern in this.WatchedFilePatterns)
						watcher.AddWatcher(dir, filePattern);
				}
			}

			// Also subscribe to the projects own changes to deal with new items being added outside of the solution directory
			uint pdwCookie;
			foreach (var p in solutionService.GetProjects())
				p.AdviseHierarchyEvents(this, out pdwCookie);
		}

		private void TestContainerUpdated(object sender, EventArgs e)
		{
			cachedTestContainers = null;

			var evt = TestContainersUpdatedInternal;
			if (evt != null)
				evt(this, EventArgs.Empty);
		}

		public event EventHandler TestContainersUpdatedInternal;

		public event EventHandler TestContainersUpdated;

		#region IDisposable Cruft

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (watcher != null)
				{
					watcher.Dispose();
					watcher = null;
				}
			}
		}

		#endregion

		#region IVsSolutionEvents Cruft

		int IVsSolutionEvents.OnAfterCloseSolution(object pUnkReserved)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
		{
			TestContainerUpdated(this, EventArgs.Empty);
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		#endregion

		#region IVsHierarchyEvents Cruft

		int IVsHierarchyEvents.OnInvalidateIcon(IntPtr hicon)
		{
			return VSConstants.S_OK;
		}

		int IVsHierarchyEvents.OnInvalidateItems(uint itemidParent)
		{
			return VSConstants.S_OK;
		}

		int IVsHierarchyEvents.OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded)
		{
			TestContainerUpdated(this, EventArgs.Empty);
			return VSConstants.S_OK;
		}

		int IVsHierarchyEvents.OnItemDeleted(uint itemid)
		{
			TestContainerUpdated(this, EventArgs.Empty);
			return VSConstants.S_OK;
		}

		int IVsHierarchyEvents.OnItemsAppended(uint itemidParent)
		{
			return VSConstants.S_OK;
		}

		int IVsHierarchyEvents.OnPropertyChanged(uint itemid, int propid, uint flags)
		{
			return VSConstants.S_OK;
		}

		#endregion
	}
}
