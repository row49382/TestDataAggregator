using System;
using System.Collections.Generic;
using System.Text;
using TestInformationAggregator.Models;

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
			TestInformationBuilderBase builder;

			switch (fileType.ToLower())
			{
				case "csv":
					{
						builder = new TestInformationCSVBuilder(headers, responseUtility, builderOptions);
						break;
					}
				case "html":
					{
						throw new NotImplementedException("html not implemented yet. Check back soon!");
					}
				default:
					{
						throw new Exception("Builder type not supported. Only html and csv are currently supported types");
					}
			}

			return builder;
		}
	}
}
