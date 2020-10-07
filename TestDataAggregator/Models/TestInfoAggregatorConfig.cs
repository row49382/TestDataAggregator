using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using TestInformationAggregator.Services;

namespace TestInformationAggregator.Models
{
	/// <summary>
	/// Class to hold the configuration properties in the Config.json class
	/// </summary>
	public class TestInfoAggregatorConfig
	{
		private readonly string nullExceptionMessage = "Value {0} from TestInfoAgreggatorConfig ctor cannot be null. Set the value in the Config.json file.";

		[JsonConstructor]
		public TestInfoAggregatorConfig(
			string organization, 
			string project, 
			string personalAccessToken,
			string outputDirectory, 
			Dictionary<string, bool> builderOptions,
			Dictionary<string, string> oDataQueries,
			string fileReportType)
		{
			Requires.NotNull(organization, string.Format(this.nullExceptionMessage, "organization"));
			Requires.NotNull(project, string.Format(this.nullExceptionMessage, "project"));
			Requires.NotNull(personalAccessToken, string.Format(this.nullExceptionMessage, "personalAccessToken"));

			this.Organization = organization;
			this.Project = project;
			this.PersonalAccessToken = personalAccessToken;
			this.BuilderOptions = builderOptions;
			this.ODataQueries = oDataQueries;

			this.OutputDirectory = string.IsNullOrEmpty(outputDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : outputDirectory;
			this.FileReportType = string.IsNullOrEmpty(fileReportType) ? "csv" : fileReportType;
		}

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

		/// <summary>
		/// The file type of the report generated (currently only supports csv and html)
		/// </summary>
		public string FileReportType { get; set; }
	}
}
