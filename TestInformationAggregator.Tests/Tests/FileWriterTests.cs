using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestInformationAggregator.Services;
using TestInformationAggregator.Tests.Services;

namespace TestInformationAggregator.Tests.Tests
{
    [TestFixture]
    public class FileWriterTests
    {
        private readonly string outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FileWriterOutputTests");

        [SetUp]
        public void CreateOutputDirectoryIfDoesNotExist()
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
        }

        [TearDown]
        public void DeleteFilesAfterRun()
        {
            foreach (var filePath in Directory.GetFiles(outputDirectory))
            {
                File.Delete(filePath);
            }

            Directory.Delete(this.outputDirectory);
        }

        [Test, TestCaseSource("GetReportFileTypes")]
        public void TestWriteOutputTypeToFile(string fileExtension, string contents)
        {
            string file = $"test.{fileExtension}";
            FileWriter.Write(this.outputDirectory, file, contents);

            Assert.IsTrue(File.Exists(Path.Combine(this.outputDirectory, file)));
            Assert.IsTrue(File.ReadAllText(Path.Combine(this.outputDirectory, file)).Equals(contents));
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void TestRequiresFilePathNotNullOrEmpty(string filePathValue)
        {
            string filePath = filePathValue;
            string fileName = "sample.txt";

            Assert.Throws<ArgumentException>(
                () => FileWriter.Write(filePath, fileName, string.Empty));
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void TestRequiresFileNameNotNullOrEmpty(string fileNameValue)
        {
            string fileName = fileNameValue;

            Assert.Throws<ArgumentException>(
                () => FileWriter.Write(this.outputDirectory, fileName, string.Empty));
        }

        [Test]
        public void TestDirectoryDoesNotExist()
        {
            string badDirectory = "badDirectory";
            string expectedMessage = $"directory was not found for path {badDirectory}";

            var exception = Assert.Throws<DirectoryNotFoundException>(
                () => FileWriter.Write(badDirectory, "sample.txt", string.Empty));

            Assert.AreEqual(expectedMessage, exception.Message);
        }

        /// <summary>
        /// Gets the data source for testing the report file types
        /// </summary>
        /// <returns> The data source for reporting file types </returns>
        private static IEnumerable<object[]> GetReportFileTypes()
        {
            var fileTestingProps = DataDrivenCSVReader.GetData("reportFileTypes.csv");

            foreach (var fileTestingProp in fileTestingProps)
            {
                yield return
                    new object[]
                    {
                        fileTestingProp[1],
                        fileTestingProp[2]
                    };
            }
        }
    }
}
