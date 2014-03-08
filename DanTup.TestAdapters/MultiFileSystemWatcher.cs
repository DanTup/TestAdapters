using System;
using System.Collections.Generic;
using System.IO;

namespace DanTup.TestAdapters
{
	/// <summary>
	/// A FileSystemWatcher that handles multiple patterns (by internally using multiple watchers).
	/// </summary>
	class MultiFileSystemWatcher : IDisposable
	{
		List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
		public event FileSystemEventHandler FileChanged;

		public void AddWatcher(string directory, string filePattern)
		{
			// HACK: Sometimes VS tells us of solution dirs that don't exist (possibly unsaved filesystem-based web sites?), but
			// the FileSystemWatcher cries if we ask it to monitor that :(
			if (!Directory.Exists(directory))
				return;

			var watcher = new FileSystemWatcher(directory, filePattern);
			watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;
			watcher.IncludeSubdirectories = true;
			watcher.Created += OnFileChanged;
			watcher.Changed += OnFileChanged;
			watcher.Deleted += OnFileChanged;
			watcher.Renamed += OnFileChanged;
			watcher.EnableRaisingEvents = true; // LOL; apparently you need this

			watchers.Add(watcher);
		}

		void OnFileChanged(object sender, FileSystemEventArgs e)
		{
			var evt = FileChanged;
			if (evt != null)
				evt(sender, e);
		}

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
				if (watchers != null)
				{
					foreach (var watcher in watchers)
						watcher.Dispose();
					watchers = null;
				}
			}
		}

		#endregion
	}
}
