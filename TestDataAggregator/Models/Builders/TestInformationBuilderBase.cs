using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TestInformationAggregator.Services;
using BuilderMethodOptions = TestInformationAggregator.Constants.BuilderOptions;

namespace TestInformationAggregator.Models
{
	public abstract class TestInformationBuilderBase
	{
		/// <summary>
		/// Gets or sets the headers for the TestInformation
		/// </summary>
		protected string TestInformationHeaders { get; set; }

		/// <summary>
		/// Gets or sets the TestInformation value that is being built to a csv
		/// </summary>
		protected List<TestInformation> TestInformation { get; set; }

		/// <summary>
		/// Gets or sets the Azure Analytics Response Utility
		/// </summary>
		protected AzureAnalyticsResponseUtility ResponseUtility { get; set; }

		/// <summary>
		/// Gets or sets the builder options
		/// </summary>
		protected Dictionary<string, bool> BuilderOptions { get; set; }

		/// <summary>
		/// Creates the TestInformationCSVBuilder
		/// </summary>
		/// <param name="headers"> The headers </param>
		/// <param name="responseUtility"> The azure analytics response utility </param>
		protected TestInformationBuilderBase(string headers, AzureAnalyticsResponseUtility responseUtility, Dictionary<string, bool> builderOptions)
		{
			this.TestInformationHeaders = headers;
			this.TestInformation = new List<TestInformation>();
			this.ResponseUtility = responseUtility;
			this.BuilderOptions = builderOptions;
		}

		/// <summary>
		/// Builds the test information report. Established by the implementation
		/// </summary>
		/// <returns> The built report </returns>
		public abstract string Build();

		/// <summary>
		/// Adds any test cases that weren't joined to a test result from the closed test cases entity into
		/// the test information for reporting. 
		/// </summary>
		/// <param name="closedTestCasesWithLinks">The closed test case workitems containing their respective links </param>
		/// <returns> the builder </returns>
		public TestInformationBuilderBase AddTestCasesWithoutTestResults(IEnumerable<JToken> closedTestCasesWithLinks)
		{
			if (this.IsOptionSelected(BuilderMethodOptions.ADD_TEST_CASES_WITHOUT_TEST_RESULTS))
			{
				var testCaseWorkItemsWithoutTestResults = closedTestCasesWithLinks
					.Where(x => !this.TestInformation.Any(y => (string)x["Title"] == y.TestName))
					.Select(x => new TestInformation()
					{
						WorkItemID = Convert.ToInt32(x["WorkItemId"]),
						TestName = (string)x["Title"],
						Priority = Convert.ToInt32(x["Priority"]),

						// we are hard coding the Active outcome to easily filter
						// all the test cases without test results in the report
						Outcome = "Active"
					});

				foreach (var testCaseWorkItemWithoutTestResults in testCaseWorkItemsWithoutTestResults)
				{
					this.TestInformation.Add(testCaseWorkItemWithoutTestResults);
				}
			}

			return this;
		}

		/// <summary>
		/// Joins the test result and the test cases
		/// </summary>
		/// <param name="testResults">The test results </param>
		/// <param name="testCases"> the test cases </param>
		/// <returns> the builder </returns>
		public TestInformationBuilderBase JoinTestResultAndTestCases(JToken testResults, JToken testCases)
		{
			// SK properties on the azure devops apis are similar to primary/foreign keys, as such, we can join
			// the results based on the TestSK property
			this.TestInformation.AddRange(this.ResponseUtility.JoinTestResultToTestcases(testResults, testCases));
			return this;
		}

		/// <summary>
		/// Filters the not applicable test results from the collection
		/// </summary>
		/// <returns> the builder </returns>
		public TestInformationBuilderBase FilterNotApplicableTestResults()
		{
			if (this.IsOptionSelected(BuilderMethodOptions.FILTER_NOT_APPLICABLE_TEST_RESULTS))
			{
				this.TestInformation = this.TestInformation.Where(x => x.Outcome != "NotApplicable").ToList();
			}

			return this;
		}

		/// <summary>
		/// Sets the remaining properties that could not be set from joining the test result and the test cases:
		/// WorkItemID, UserStoryLink and BugLinks
		/// </summary>
		/// <param name="closedTestCasesWithLinks"> The closed test case workitems containing their respective links</param>
		/// <param name="workItemRevisions"> All workItem revision history, needed if the work item name doesn't match the test name</param>
		/// <param name="workItems"> All workitems </param>
		/// <returns> the builder </returns>
		public TestInformationBuilderBase SetRemainingTestInfoProperties(IEnumerable<JToken> closedTestCasesWithLinks, JToken workItemRevisions, JArray workItems)
		{
			foreach (var testInformationInstance in this.TestInformation)
			{
				// Title property on the WorkItem object matches (in most cases) the TestName property on the Test object
				JToken workItemMatch = closedTestCasesWithLinks.FirstOrDefault(x => string.Equals((string)x["Title"], testInformationInstance.TestName));

				// if workItemMatch at this point is null, we need the check the revision history because
				// the TestName on results uses the original name of the workitem while the WorkItem uses the most recent name
				if (workItemMatch == null)
				{
					workItemMatch = this.ResponseUtility.GetMatchingWorkItemFromEarliestRevisionTitle(workItemRevisions, closedTestCasesWithLinks, testInformationInstance);
				}

				if (workItemMatch != null)
				{
					List<string> linkTypesFilter = new List<string> { "Tests" };

					IEnumerable<JToken> bugLinks = this.ResponseUtility.GetLinksFromMatch(workItemMatch, workItems, "Bug", linkTypesFilter);
					IEnumerable<JToken> userStoryLinks = this.ResponseUtility.GetLinksFromMatch(workItemMatch, workItems, "User Story", linkTypesFilter);
					Dictionary<string, List<int>> bugLinksByState = this.ResponseUtility.GroupWorkItemsByState(bugLinks);

					int workItemId = Convert.ToInt32(workItemMatch?["WorkItemId"]);
					IEnumerable<int> userStoryIds = userStoryLinks.Select(x => Convert.ToInt32(x?["WorkItemId"]));

					// The WorkItem will always have the latest test name. Set this as the TestName to ensure
					// the latest test name is written to the report
					string workItemTitle = (string)workItemMatch["Title"];

					testInformationInstance.WorkItemID = workItemId;
					testInformationInstance.UserStoryIdLinks = userStoryIds;
					testInformationInstance.LinkedBugsByState = bugLinksByState;
					testInformationInstance.TestName = workItemTitle;
				}
			}

			return this;
		}

		/// <summary>
		/// Filters outs the closed test cases
		/// </summary>
		/// <returns> the builder </returns>
		public TestInformationBuilderBase FilterClosedTestCases()
		{
			// Any WorkItemID that is not set is  because the test case was closed
			if (this.IsOptionSelected(BuilderMethodOptions.FILTER_CLOSED_TEST_CASES))
			{
				this.TestInformation = this.TestInformation.Where(x => x.WorkItemID != 0).ToList();
			}

			return this;
		}

		/// <summary>
		/// Filters out only the most recent test result for any given test case
		/// </summary>
		/// <returns> the builder </returns>
		public TestInformationBuilderBase KeepMostRecentTestResults()
		{
			if (this.IsOptionSelected(BuilderMethodOptions.KEEP_MOST_RECENT_TEST_RESULTS))
			{
				this.TestInformation = this.TestInformation
					.OrderByDescending(x => x.CompletedDate)
					.GroupBy(x => x.WorkItemID, (key, group) => group.First())
					.ToList();
			}

			return this;
		}


		/// <summary>
		/// Checks if the builder option is selected from the Config.json
		/// </summary>
		/// <param name="builderOption"> the name of the builder option</param>
		/// <returns> if the option is selected </returns>
		private bool IsOptionSelected(string builderOption)
		{
			if (this.BuilderOptions.ContainsKey(builderOption))
			{
				return this.BuilderOptions[builderOption];
			}

			return true;
		}
	}
}
