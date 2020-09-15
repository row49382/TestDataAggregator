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
				File.ReadAllText(Path.Combine(this.GetAssemblyPath(), configFileName)));

			if (string.IsNullOrEmpty(this.Configuration.OutputDirectory))
			{
				this.Configuration.OutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
		}

		/// <summary>
		/// Gets the executing assembly path
		/// </summary>
		/// <returns> The executing assembly path</returns>
		private string GetAssemblyPath()
		{
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().Location);
			return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
		}
	}
}
