using System;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace DanTup.TestAdapters
{
	/// <summary>
	/// Extensions for getting Visual Studio projects, and their items; since the API for this is crufty.
	/// </summary>
	static class VsExtensions
	{
		public static IEnumerable<IVsHierarchy> GetProjects(this IVsSolution solutionService)
		{
			IEnumHierarchies projects;
			var result = solutionService.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_ALLINSOLUTION, Guid.Empty, out projects);

			if (result == VSConstants.S_OK)
			{
				IVsHierarchy[] hierarchy = new IVsHierarchy[1] { null };
				uint fetched = 0;
				for (projects.Reset(); projects.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched == 1; /*nothing*/)
				{
					yield return hierarchy[0];
				}
			}
		}

		public static IEnumerable<string> GetProjectItems(this IVsHierarchy project)
		{
			return project.GetProjectItems(VSConstants.VSITEMID_ROOT);
		}

		public static IEnumerable<string> GetProjectItems(this IVsHierarchy project, uint itemID)
		{
			object item;

			// Get first item
			project.GetProperty(itemID, (int)__VSHPROPID.VSHPROPID_FirstVisibleChild, out item);

			while (item != null)
			{
				string canonicalName;
				project.GetCanonicalName((uint)(int)item, out canonicalName);
				if (!string.IsNullOrWhiteSpace(canonicalName))
					yield return canonicalName;

				// Call recursively for children
				foreach (var child in project.GetProjectItems((uint)(int)item))
					yield return child;

				// Get next sibling
				project.GetProperty((uint)(int)item, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling, out item);
			}
		}
	}
}
