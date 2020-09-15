using System.Collections.Generic;

namespace TestInformationAggregator.Models
{
	/// <summary>
	/// Class to hold the configuration properties in the Config.json class
	/// </summary>
	public class TestInfoAggregatorConfig
	{
		/// <summary>
		/// The Azure DevOps Organization
		/// </summary>
		public string Organization { get; set; }

		/// <summary>
		/// The Azure DevOps project
		/// </summary>
		public string Project { get; set; }

		/// <summary>
		/// The personal access token to setup basic authorization for
		/// the api queries. 
		/// 
		/// To generate your own, follow the instructions here: 
		/// https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=preview-page
		/// and make sure you are authorized to Query the Analytics api
		/// </summary>
		public string PersonalAccessToken { get; set; }

		/// <summary>
		/// The output directory that the file will be written to
		/// </summary>
		public string OutputDirectory { get; set; }

		/// <summary>
		/// The Builder Options that are chosen to be applied in the report
		/// </summary>
		public Dictionary<string, bool> BuilderOptions { get; set; }

		/// <summary>
		/// The Odata queries that can be configured on the api calls
		/// </summary>
		public Dictionary<string, string> ODataQueries { get; set; }
	}
}
