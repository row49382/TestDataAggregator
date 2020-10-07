using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System;
using TestInformationAggregator.Models;

namespace TestInformationAggregator.Services
{
	/// <summary>
	/// Class to wrap the Configuration
	/// </summary>
	public class JsonConfigManager
	{
		/// <summary>
		/// Gets or sets the Configuration
		/// </summary>
		public TestInfoAggregatorConfig Configuration { get; set; }

		/// <summary>
		/// Builds the JsonConfigManager. Sets the Configuration from the provided config file name.
		/// Defaults the OutputDirectory to Desktop if not provided
		/// </summary>
		/// <param name="configFileName"> The config file name</param>
		public JsonConfigManager(string configFileName)
		{
			this.Configuration = JsonConvert.DeserializeObject<TestInfoAggregatorConfig>(
				File.ReadAllText(Path.Combine(AssemblyPathFinder.GetAssemblyDirectoryPath(), configFileName)));
		}
	}
}
