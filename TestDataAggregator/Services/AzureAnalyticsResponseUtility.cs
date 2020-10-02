using TestInformationAggregator.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestInformationAggregator.Services
{
	/// <summary>
	/// Utility class to handle logic for AzureDevops Analytics API responses
	/// </summary>
	public class AzureAnalyticsResponseUtility
	{
		/// <summary>
		/// Joins the test results to the test cases using the TestSK value
		/// </summary>
		/// <param name="testResults"> The test results to join </param>
		/// <param name="testCases"> the test cases to join </param>
		/// <returns> the joined test information </returns>
		public IEnumerable<TestInformation> JoinTestResultToTestcases(JToken testResults, JToken testCases)
		{
			return from testResult in testResults
				   join testCase in testCases
				   on (string)testResult["TestSK"] equals (string)testCase["TestSK"]
				   select new TestInformation()
				   {
					   CompletedDate = Convert.ToDateTime(testResult["CompletedDate"]),
					   TestSK = Convert.ToInt32(testResult["TestSK"]),
					   TestRunType = (string)testResult["TestRunType"],
					   Outcome = (string)testResult["Outcome"],
					   TestName = (string)testCase["TestName"],
					   TestOwner = (string)testCase["TestOwner"],
					   Priority = Convert.ToInt32(testCase["Priority"])
				   };
		}

		/// <summary>
		/// Gets the matching work item to the test information by looking at the revision history
		/// and seeing if any title change matches the name on the test information object
		/// </summary>
		/// <param name="workItemRevisions"> the work item revisions</param>
		/// <param name="testCasesWithLinks"> the test cases with their links attached</param>
		/// <param name="testInformationInstance"> the current test information attempting to be matched to a work item</param>
		/// <returns> The matching workitem found that has a Title property that matches the test information's TestName property </returns>
		public JToken GetMatchingWorkItemFromEarliestRevisionTitle(JToken workItemRevisions, IEnumerable<JToken> testCasesWithLinks, TestInformation testInformationInstance)
		{
			int matchingRevisionId = Convert.ToInt32(workItemRevisions.FirstOrDefault(x => (string)x["Title"] == testInformationInstance.TestName)?["WorkItemId"]);
			return testCasesWithLinks.FirstOrDefault(x => Convert.ToInt32(x["WorkItemId"]) == matchingRevisionId);
		}


		/// <summary>
		/// Groups the workItems by their state
		/// </summary>
		/// <param name="workItems"> The workItems being grouped</param>
		/// <returns> The grouped workItems</returns>
		public Dictionary<string, List<int>> GroupWorkItemsByState(IEnumerable<JToken> workItems)
		{
			Dictionary<string, List<int>> workItemsByState = new Dictionary<string, List<int>>();

			foreach (var workItem in workItems)
			{
				string workItemState = (string)workItem["State"];

				if (!workItemsByState.ContainsKey(workItemState))
				{
					workItemsByState[workItemState] = new List<int>();
				}

				var workItemIdsForState = workItemsByState[workItemState];
				workItemIdsForState.Add(Convert.ToInt32(workItem["WorkItemId"]));
				workItemsByState[workItemState] = workItemIdsForState;
			}

			return workItemsByState;
		}

		/// <summary>
		/// Finds all bug ids linked to the work Item match found
		/// </summary>
		/// <param name="workItemMatch"> The work item found that is having it's bugs searched for</param>
		/// <param name="workItems">The list of all work items</param>
		/// <param name="workItemType"> The workitem type to filter by</param>
		/// <param name="linkTypes"> The link types to filter the link by. If not provided the comparison is ignored</param>
		/// <returns> The bug ids attached to the matched work item</returns>
		public IEnumerable<JToken> GetLinksFromMatch(JToken workItemMatch, JArray workItems, string workItemType, List<string> linkTypes = null)
		{
			var links = new List<JToken>();
			linkTypes ??= new List<string>();

			foreach (var link in workItemMatch["Links"])
			{
				// to find the linked work item, the Links property contains a TargetWorkItemId which we use to find the 
				// full work item object from the workItems collection
				var targetWorkItem = workItems
					.FirstOrDefault(x => (string)x["WorkItemId"] == (string)link["TargetWorkItemId"]);

				if (targetWorkItem != null && 
					(string)targetWorkItem["WorkItemType"] == workItemType &&
					!linkTypes.Any() ? true : linkTypes.Any(x => x == (string)link["LinkTypeName"]))
				{
					links.Add(targetWorkItem);
				}
			}

			return links;
		}
	}
}