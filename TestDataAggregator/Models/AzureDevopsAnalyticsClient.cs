using TestInformationAggregator.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestInformationAggregator.Models
{
	/// <summary>
	/// HttpClient Wrapper class for Azure DevOps Analtyics api queries
	/// </summary>
	public class AzureDevopsAnalyticsClient
	{
		/// <summary>
		/// Gets or sets the httpclient
		/// </summary>
		private HttpClient Client { get; set; }

		/// <summary>
		/// Creates the HttpClient wrapper class for Azure DevOps Analytics api queries
		/// </summary>
		/// <param name="organization"> The Azure DevOps organization </param>
		/// <param name="project"> The Azure DevOps project </param>
		/// <param name="personalAccessToken"> The personal access token to authorize requests to Azure DevOps Analytics api </param>
		public AzureDevopsAnalyticsClient(string organization, string project, string personalAccessToken)
		{
			this.Client = this.GetAzureDevopsAnalticsHttpClient(organization, project, personalAccessToken);
		}

		/// <summary>
		/// Gets the work items value property
		/// </summary>
		/// <param name="oDataQuery"> the odata query string </param>
		/// <returns> The work items value property </returns>
		public JArray GetWorkItems(string oDataQuery)
		{
			return (JArray)this.GetValue("WorkItems", oDataQuery);
		}

		/// <summary>
		/// Gets the work item revisions value property
		/// </summary>
		/// <param name="oDataQuery"> the odata query string </param>
		/// <returns> the work item revisions value property </returns>
		public JToken GetWorkItemRevisions(string oDataQuery)
		{
			return this.GetValue("WorkItemRevisions", oDataQuery);
		}

		/// <summary>
		/// Gets all closed test case with their links from the workitem value property
		/// </summary>
		/// <returns> The closed test case work items with links </returns>
		public IEnumerable<JToken> GetClosedTestCasesWorktItemsWithLinks()
		{
			return this.GetWorkItems("$expand=Links($select=SourceWorkItemId,TargetWorkItemId,LinkTypeName)")
				.Where(x => (string)x["WorkItemType"] == "Test Case")
				.Where(y => (string)y["State"] != WorkItemStates.CLOSED);
		}

		/// <summary>
		/// Gets the test results from the value property
		/// </summary>
		/// <param name="oDataQuery"> the odata query string </param>
		/// <returns> The test results from the value property </returns>
		public JToken GetTestResults(string oDataQuery)
		{
			return this.GetValue("testResults", oDataQuery);
		}

		/// <summary>
		/// Gets the test cases from the value property
		/// </summary>
		/// <param name="oDataQuery"> the odata query string </param>
		/// <returns> The test cases from the value property </returns>
		public JToken GetTestCases(string oDataQuery)
		{
			return this.GetValue("Tests", oDataQuery);
		}

		/// <summary>
		/// Awaits the response from the endpoint and gets the value property from the 
		/// json response from the analytics api
		/// </summary>
		/// <param name="endpoint"> the endpoint to query</param>
		/// <param name="oDataQuery"> the odata query string </param>
		/// <returns> The value property of the query </returns>
		private JToken GetValue(string endpoint, string oDataQuery = "")
		{
			string fullEndpoint = string.IsNullOrEmpty(oDataQuery) ? endpoint : $"{endpoint}?{oDataQuery}";

			return JToken.Parse(this.Client.GetAsync(fullEndpoint).Result.Content.ReadAsStringAsync().GetAwaiter().GetResult())
				.SelectToken("value");
		}

		/// <summary>
		/// Creates the http client for azure devops analytics queries 
		/// </summary>
		/// <param name="personalAccessToken"> The personal access token needed to authorize our request </param>
		/// <returns> The authorized httclient </returns>
		private HttpClient GetAzureDevopsAnalticsHttpClient(string organization, string project, string personalAccessToken)
		{
			var client = new HttpClient()
			{
				BaseAddress = new Uri($"https://analytics.dev.azure.com/{organization}/{project}/_odata/v3.0-preview/")
			};

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
				Convert.ToBase64String(
					ASCIIEncoding.ASCII.GetBytes(
						string.Format("{0}:{1}", string.Empty, personalAccessToken))));

			return client;
		}
	}
}
