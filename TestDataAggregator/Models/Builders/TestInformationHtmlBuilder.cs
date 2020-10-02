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
		/// <param name="delimiter"> The delimiter for the test info to split on for table generation</param>
		public TestInformationHtmlBuilder(string headers, AzureAnalyticsResponseUtility responseUtility, Dictionary<string, bool> builderOptions, char delimiter)
			: base(headers, responseUtility, builderOptions, delimiter) { }

		/// <summary>
		/// Builds the html using the TestInformation ToString
		/// </summary>
		/// <returns> The built html report </returns>
		public override string Build()
		{
			StringBuilder strBuilder = new StringBuilder();

			strBuilder.AppendLine("<html><body><table>");
			strBuilder.AppendLine(this.BuildTableEntry("th", this.TestInformationHeaders.Split(',')));

			foreach (var testInformationInstance in this.TestInformation)
			{
				strBuilder.AppendLine(this.BuildTableEntry("td", testInformationInstance.ToString().Split(this.Delimiter)));
			}

			strBuilder.AppendLine("</body></table></html>");

			return strBuilder.ToString();
		}

		/// <summary>
		/// Builds each table entry
		/// </summary>
		/// <param name="tableElementType"> The table element type (either th or td) </param>
		/// <param name="values"> The values to write into the tabl </param>
		/// <returns> The table entry </returns>
		private string BuildTableEntry(string tableElementType, string[] values)
		{
			StringBuilder tableEntryBuilder = new StringBuilder();

			tableEntryBuilder.AppendLine("<tr>");

			foreach (var value in values)
			{
				tableEntryBuilder.Append($"<{tableElementType}>");
				tableEntryBuilder.Append($"{value}");
				tableEntryBuilder.AppendLine($"</{tableElementType}>");
			}

			tableEntryBuilder.AppendLine("</tr>");

			return tableEntryBuilder.ToString();
		}
	}
}
