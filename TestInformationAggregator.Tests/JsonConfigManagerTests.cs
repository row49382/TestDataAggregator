using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestInformationAggregator.Models;
using TestInformationAggregator.Services;
using TestInformationAggregator.Tests.Factories;

namespace TestInformationAggregator.Tests
{
    public class JsonConfigManagerTests
    {
        private readonly JsonConfigManagerFactory jsonConfigManagerFactory = new JsonConfigManagerFactory(); 

        [Test]
        public void TestLoadJsonConfig()
        {
            var configManager = this.jsonConfigManagerFactory.GetEntity("Data/Configs/ValidConfig.json");
            this.AssertAllConfigurationProperties(configManager.Configuration);
        }

        [Test, TestCaseSource("GetEmptyFileTestingVariables")]
        public void TestLoadEmptyConfigThrowsArgumentExceptions(string fileLocation, string messageThrown)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => this.jsonConfigManagerFactory.GetEntity(fileLocation));

            Assert.AreEqual(messageThrown, exception.Message);
        }

        [Test]
        public void TestLoadNonExistentFile()
        {
            Assert.Throws<FileNotFoundException>(() => this.jsonConfigManagerFactory.GetEntity("Data/Configs/Config_NoExist.json"));
        }

        [Test]
        public void TestLoadInvalidJsonFile()
        {
            Assert.Throws<JsonReaderException>(() => this.jsonConfigManagerFactory.GetEntity("Data/Configs/InvalidConfig.json"));
        }

        [Test]
        public void TestDefaultFileReportType()
        {
            var configManager = this.jsonConfigManagerFactory.GetEntity("Data/Configs/DefaultFileReportTypeValueConfig.json");
            Assert.AreEqual("csv", configManager.Configuration.FileReportType);
        }

        [Test]
        public void TestDefaultOutputDirectory()
        {
            var configManager = this.jsonConfigManagerFactory.GetEntity("Data/Configs/DefaultOutputDirectoryValueConfig.json");
            Assert.AreEqual(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), configManager.Configuration.OutputDirectory);
        }

        /// <summary>
        /// Asserts all configuration properties when values are set
        /// </summary>
        /// <param name="config"> The config to assert on </param>
        private void AssertAllConfigurationProperties(TestInfoAggregatorConfig config)
        {
            Assert.NotNull(config);

            Assert.IsTrue(!string.IsNullOrEmpty(config.Organization));
            Assert.IsTrue(!string.IsNullOrEmpty(config.Project));
            Assert.IsTrue(!string.IsNullOrEmpty(config.PersonalAccessToken));

            Assert.IsTrue(!string.IsNullOrEmpty(config.OutputDirectory));
            Assert.IsTrue(!string.IsNullOrEmpty(config.FileReportType));

            Assert.IsTrue(config.BuilderOptions["FilterNotApplicableTestResults"]);
            Assert.IsTrue(config.BuilderOptions["FilterClosedTestCases"]);
            Assert.IsTrue(config.BuilderOptions["KeepMostRecentTestResults"]);
            Assert.IsTrue(config.BuilderOptions["AddTestCasesWithoutTestResults"]);

            Assert.IsTrue(!string.IsNullOrEmpty(config.ODataQueries["TestResults"]));
            Assert.IsTrue(!string.IsNullOrEmpty(config.ODataQueries["WorkItems"]));
            Assert.IsTrue(!string.IsNullOrEmpty(config.ODataQueries["TestCases"]));
            Assert.IsTrue(!string.IsNullOrEmpty(config.ODataQueries["WorkItemRevisions"]));
        }

        /// <summary>
        /// Data provider to get the empty json locations from the csv file
        /// </summary>
        /// <returns> The empty json config files for testing </returns>
        private static IEnumerable<object[]> GetEmptyFileTestingVariables()
        {
            IEnumerable<string[]> emptyFileTestingProps = File.ReadAllLines(
                Path.Combine(AssemblyPathFinder.GetAssemblyDirectoryPath(), "Data/DataDrivenCSV/emptyConfigs.csv"))
                .Skip(1)
                .Select(x => x.Split(','));

            foreach (var emptyFileTestingProp in emptyFileTestingProps)
            {
                yield return 
                    new object[]
                    {
                        emptyFileTestingProp[1],
                        emptyFileTestingProp[2]
                    };
            }
        }
    }
}
