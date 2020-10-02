using TestInformationAggregator.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestInformationAggregator.Models
{
	/// <summary>
	/// Represents the joined Test Case and Test Result information
	/// </summary>
	public class TestInformation
	{
		/// <summary>
		/// The WorkItemID of the test case
		/// </summary>
		public int WorkItemID { get; set; }

		/// <summary>
		/// The completed date on the test result
		/// </summary>
		public DateTime CompletedDate { get; set; }

		/// <summary>
		/// The UserStory ids linked to the test case
		/// </summary>
		public IEnumerable<int> UserStoryIdLinks { get; set; }

		/// <summary>
		/// The TestSK for joining the Test Case to the Test Result
		/// </summary>
		public int TestSK { get; set; }

		/// <summary>
		/// The Test Run Type of the Test Result
		/// </summary>
		public string TestRunType { get; set; }

		/// <summary>
		/// The outcome of the Test Result
		/// </summary>
		public string Outcome { get; set; }

		/// <summary>
		/// The name of the Test Case
		/// </summary>
		public string TestName { get; set; }

		/// <summary>
		/// The owner of the Test Case
		/// </summary>
		public string TestOwner { get; set; }

		/// <summary>
		/// The Priority of the Test Case
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// A list of any bug WorkItemIDs attached to the Test Case grouped by
		/// the bug state on the WorkItem
		/// </summary>
		public Dictionary<string, List<int>> LinkedBugsByState { get; set; }

		/// <summary>
		/// To String method to represent properties in appropriately ordered
		/// comma delimited string for csv generation
		/// </summary>
		/// <returns> The object in ordered csv format</returns>
		public override string ToString()
		{
			return $"{this.WorkItemID}," +
				   $"{this.GetLinkedIdsAsCSV(this.UserStoryIdLinks)}," +
				   $"{this.CompletedDate}," +
				   $"{this.TestSK}," +
				   $"{this.TestRunType}," +
				   $"{this.Outcome}," +
				   $"\"{this.TestName}\"," +
				   $"{this.TestOwner}," +
				   $"{this.Priority}," +
				   $"{this.GetLinkedIdsAsCSV(this.GetLinkedBugs(WorkItemStates.NEW))}," +
				   $"{this.GetLinkedIdsAsCSV(this.GetLinkedBugs(WorkItemStates.ACTIVE))}," +
				   $"{this.GetLinkedIdsAsCSV(this.GetLinkedBugs(WorkItemStates.RESOLVED))}," +
				   $"{this.GetLinkedIdsAsCSV(this.GetLinkedBugs(WorkItemStates.CLOSED))}";
		}

		/// <summary>
		/// Gets the linked ids into a comma separated list
		/// </summary>
		/// <param name="linkedIds"> the ids being aggregated </param>
		/// <returns> the linked ids as a comma separated list </returns>
		private string GetLinkedIdsAsCSV(IEnumerable<int> linkedIds)
		{
			return $"\"{linkedIds?.Select(x => x.ToString()).Aggregate((acc, curr) => acc + ", " + curr)}\"";
		}

		/// <summary>
		/// Gets the linked bugs using the provided state. This method is used to avoid a KeyNotFoundException
		/// if there are no bugs recorded for that state and instead return null
		/// </summary>
		/// <param name="state"> the state to try and get the bug ids from</param>
		/// <returns> the linked bugs or null if the key doesn't exist in the dictionary </returns>
		private IEnumerable<int> GetLinkedBugs(string state)
		{
			List<int> bugIds = null;

			if (this.LinkedBugsByState != null)
			{
				this.LinkedBugsByState.TryGetValue(state, out bugIds);
			}

			return bugIds;
		}
	}
}
