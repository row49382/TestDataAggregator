using System;
using System.Collections.Generic;
using System.Text;
using TestInformationAggregator.Models;
using TestInformationAggregator.Models.Builders;

namespace TestInformationAggregator.Services
{
	public static class TestInformationBuilderFactory
	{
		/// <summary>
		/// Gets the builder based on the file type provided
		/// </summary>
		/// <param name="fileType"> the file type requested</param>
		/// <param name="headers"> the headers of the report</param>
		/// <param name="responseUtility"> the response utility for handling api repsonses</param>
		/// <param name="builderOptions"> the builder options </param>
		/// <returns> The builder based on the file type requested </returns>
		public static TestInformationBuilderBase GetBuilder(
			string fileType, 
			string headers, 
			AzureAnalyticsResponseUtility responseUtility, 
			Dictionary<string, bool> builderOptions)
		{
			TestInformationBuilderBase builder = (fileType.ToLower()) switch
			{
				"csv"  => new TestInformationCSVBuilder(headers, responseUtility, builderOptions),
				"html" => new TestInformationHtmlBuilder(headers, responseUtility, builderOptions),
				_ =>      throw new ArgumentException($"Builder type {fileType} not supported. Only html and csv are currently supported types"),
			};

			return builder;
		}
	}
}
