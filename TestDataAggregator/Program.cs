using TestInformationAggregator.Constants;
using TestInformationAggregator.Models;
using TestInformationAggregator.Services;
using System;
using System.Net.Http.Headers;

namespace TestInformationAggregator
{
	public class Program
	{
		/// <summary>
		/// Aggregates the test result, workId, Linked bugs by status and test case information to a csv file. 
		/// </summary>
		public static void Main(string[] args)
		{
			JsonConfigManager config = new JsonConfigManager("Config.json");

			string pat = config.Configuration.PersonalAccessToken;
			string organization = config.Configuration.Organization;
			string project = config.Configuration.Project;
			string outputDir = config.Configuration.OutputDirectory;
			string fileReportType = config.Configuration.FileReportType;

			string testInformationHeaders =
				$"WorkItemID,Linked UserStoryIds,CompletedDate,TestSK,TestRunType,Outcome,TestName,TestOwner,Priority," +
				$"Linked Bugs({WorkItemStates.NEW}),Linked Bugs({WorkItemStates.ACTIVE}),Linked Bugs({WorkItemStates.RESOLVED}),Linked Bugs({WorkItemStates.CLOSED})" +
				$"{Environment.NewLine}";

			AzureDevopsAnalyticsClient azureDevopsAnalyticsClient = new AzureDevopsAnalyticsClient(organization, project, pat);

			var closedTestCasesWithLinks = azureDevopsAnalyticsClient.GetClosedTestCasesWorktItemsWithLinks();

			var workItems = azureDevopsAnalyticsClient.GetWorkItems(config.Configuration.ODataQueries["WorkItems"]);
			var workItemRevisions = azureDevopsAnalyticsClient.GetWorkItemRevisions(config.Configuration.ODataQueries["WorkItemRevisions"]);

			var testResults = azureDevopsAnalyticsClient.GetTestResults(config.Configuration.ODataQueries["TestResults"]);
			var testCases = azureDevopsAnalyticsClient.GetTestCases(config.Configuration.ODataQueries["TestCases"]);

			TestInformationBuilderBase builder = TestInformationBuilderFactory.GetBuilder(
				fileReportType, 
				testInformationHeaders, 
				new AzureAnalyticsResponseUtility(), 
				config.Configuration.BuilderOptions);

			string testInformationReport = builder
				.JoinTestResultAndTestCases(testResults, testCases)
				.FilterNotApplicableTestResults()
				.SetRemainingTestInfoProperties(closedTestCasesWithLinks, workItemRevisions, workItems)
				.FilterClosedTestCases()
				.KeepMostRecentTestResults()
				.AddTestCasesWithoutTestResults(closedTestCasesWithLinks)
				.Build();

			FileWriter.Write(
				outputDir,
				$"{organization}{project}TestReport_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.{fileReportType.ToLower()}",
				testInformationReport);
		}
	}
}
