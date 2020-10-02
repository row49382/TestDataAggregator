using System;
using System.Collections.Generic;
using System.Text;
using TestInformationAggregator.Services;

namespace TestInformationAggregator.Models.Builders
{
	/// <summary>
	/// Class to build the csv output using the TestInformation
	/// </summary>
	public class TestInformationHtmlBuilder : TestInformationBuilderBase
	{
		/// <summary>
		/// Creates the TestInformationHtmlBuilder
		/// </summary>
		/// <param name="headers"> The headers </param>
		/// <param name="responseUtility"> The azure analytics response utility </param>
		/// <param name="builderOptions"> The builder options to apply based on the config values </param>
		public TestInformationHtmlBuilder(string headers, AzureAnalyticsResponseUtility responseUtility, Dictionary<string, bool> builderOptions)
			: base(headers, responseUtility, builderOptions) { }

		/// <summary>
		/// Builds the csv using the TestInformation ToString
		/// </summary>
		/// <returns> The built csv</returns>
		public override string Build()
		{
			throw new NotImplementedException();
		}
	}
}
