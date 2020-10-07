using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TestInformationAggregator.Services
{
    public static class AssemblyPathFinder
    {
		/// <summary>
		/// Gets the executing assembly path
		/// </summary>
		/// <returns> The executing assembly path</returns>
		public static string GetAssemblyDirectoryPath()
		{
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().Location);
			return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
		}
	}
}
