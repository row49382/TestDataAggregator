using TestInformationAggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestInformationAggregator.Models
{
	/// <summary>
	/// Class to build the csv output using the TestInformation
	/// </summary>
	public class TestInformationCSVBuilder : TestInformationBuilderBase
	{
		/// <summary>
		/// Creates the TestInformationCSVBuilder
		/// </summary>
		/// <param name="csvHeaders"> The csv headers </param>
		/// <param name="responseUtility"> The azure analytics response utility </param>
		/// <param name="builderOptions"> The builder options to apply based on the config values </param>
		public TestInformationCSVBuilder(string csvHeaders, AzureAnalyticsResponseUtility responseUtility, Dictionary<string, bool> builderOptions) 
			: base(csvHeaders, responseUtility, builderOptions) { }

		/// <summary>
		/// Builds the csv using the TestInformation ToString
		/// </summary>
		/// <returns> The built csv</returns>
		public override string Build()
		{
			return this.TestInformation.Aggregate(this.TestInformationHeaders, (acc, curr) => acc + curr.ToString() + Environment.NewLine);
		}
	}
}
