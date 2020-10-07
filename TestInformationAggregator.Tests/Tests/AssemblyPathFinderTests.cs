using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using TestInformationAggregator.Services;

namespace TestInformationAggregator.Tests.Tests
{
    [TestFixture]
    public class AssemblyPathFinderTests
    {
        [Test]
        public void TestGetAssemblyDirectory()
        {
            string expectedPath = Path.GetDirectoryName(Uri.UnescapeDataString(Assembly.GetExecutingAssembly().Location));
            Assert.AreEqual(expectedPath, AssemblyPathFinder.GetAssemblyDirectoryPath());
        }
    }
}
