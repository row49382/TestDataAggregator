using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestInformationAggregator.Services;

namespace TestInformationAggregator.Tests.Services
{
    public static class DataDrivenCSVReader
    {
        /// <summary>
        /// The data driven relative folder path
        /// </summary>
        private static readonly string dataDrivenCSVDirectory = "Data/DataDrivenCSV/";

        /// <summary>
        /// Gets the data from the csv used for data driving tests
        /// </summary>
        /// <param name="csvFileName"> The name of the csv for datadriving </param>
        /// <returns> The data for tests </returns>
        public static IEnumerable<object[]> GetData(string csvFileName)
        {
            return File.ReadAllLines(
                Path.Combine(AssemblyPathFinder.GetAssemblyDirectoryPath(), $"{dataDrivenCSVDirectory}{csvFileName}"))
                .Skip(1)
                .Select(x => x.Split(','));
        }
    }
}
