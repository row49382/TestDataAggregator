using TestInformationAggregator.Constants;
using TestInformationAggregator.Models;
using TestInformationAggregator.Services;
using System;

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

			string testInformationReport = new TestInformationCSVBuilder(testInformationHeaders, new AzureAnalyticsResponseUtility(), config.Configuration.BuilderOptions)
				.JoinTestResultAndTestCases(testResults, testCases)
				.FilterNotApplicableTestResults()
				.SetRemainingTestInfoProperties(closedTestCasesWithLinks, workItemRevisions, workItems)
				.FilterClosedTestCases()
				.KeepMostRecentTestResults()
				.AddTestCasesWithoutTestResults(closedTestCasesWithLinks)
				.Build();

			FileWriter.Write(
				string.IsNullOrEmpty(outputDir) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : outputDir,
				$"{organization}{project}TestReport_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.csv",
				testInformationReport);
		}
	}
}
