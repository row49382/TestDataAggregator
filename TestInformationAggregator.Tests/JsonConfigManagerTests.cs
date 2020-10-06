using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using TestInformationAggregator.Models;
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

        [Test]
        public void TestLoadEmptyConfig()
        {
            var configManager = this.jsonConfigManagerFactory.GetEntity("Data/Configs/EmptyConfig.json");
            this.AssertAllConfigurationPropertiesFromEmpty(configManager.Configuration);
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

        /// <summary>
        /// Asserts all configuration properties when values are set
        /// </summary>
        /// <param name="config"> The config to assert on</param>
        private void AssertAllConfigurationProperties(TestInfoAggregatorConfig config)
        {
            Assert.NotNull(config);

            Assert.IsTrue(!string.IsNullOrEmpty(config.Organization));
            Assert.IsTrue(!string.IsNullOrEmpty(config.Project));
            Assert.IsTrue(!string.IsNullOrEmpty(config.PersonalAccessToken));
            Assert.IsTrue(!string.IsNullOrEmpty(config.OutputDirectory));

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
        /// Asserst all configuration properties when values are not set
        /// </summary>
        /// <param name="config"> The config to assert on </param>
        private void AssertAllConfigurationPropertiesFromEmpty(TestInfoAggregatorConfig config)
        {
            Assert.NotNull(config);

            Assert.AreEqual(config.Organization, string.Empty);
            Assert.AreEqual(config.Project, string.Empty);
            Assert.AreEqual(config.PersonalAccessToken, string.Empty);
            Assert.AreEqual(config.OutputDirectory, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            Assert.IsTrue(config.BuilderOptions["FilterNotApplicableTestResults"]);
            Assert.IsTrue(config.BuilderOptions["FilterClosedTestCases"]);
            Assert.IsTrue(config.BuilderOptions["KeepMostRecentTestResults"]);
            Assert.IsTrue(config.BuilderOptions["AddTestCasesWithoutTestResults"]);

            Assert.AreEqual(config.ODataQueries["TestResults"], string.Empty);
            Assert.AreEqual(config.ODataQueries["WorkItems"], string.Empty);
            Assert.AreEqual(config.ODataQueries["TestCases"], string.Empty);
            Assert.AreEqual(config.ODataQueries["WorkItemRevisions"], string.Empty);
        }

    }
}
