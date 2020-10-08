using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestInformationAggregator.Services;

namespace TestInformationAggregator.Tests.Tests
{
    [TestFixture]
    public class RequiresTests
    {
        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void TestEmptyStringThrowsArgumentException(string value)
        {
            string str = value;
            Assert.Throws<ArgumentException>(() => Requires.NotNull(str));
        }

        [Test]
        public void TestNullObjectThrowsArgumentException()
        {
            object obj = null;
            Assert.Throws<ArgumentException>(() => Requires.NotNull(obj));
        }

        [Test]
        public void TestNoExceptionThrownForNonNullObject()
        {
            object obj = 3;
            Requires.NotNull(obj);
        }

        [Test]
        public void TestMessageUsedInThrownExceptionForNotNull()
        {
            object obj = null;
            string expectedException = "test exception.";

            var exception = Assert.Throws<ArgumentException>(() => Requires.NotNull(obj, expectedException));

            Assert.AreEqual(expectedException, exception.Message);
        }

        [Test]
        public void TestValuesInSetFound()
        {
            int value = 2;
            IEnumerable<object> set = new object[] { 1, 2, 3 };

            Requires.ValuesIn(value, set);
        }

        [Test]
        public void TestValuesNotFoundInSet()
        {
            int value = 4;
            IEnumerable<object> set = new object[] { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() => Requires.ValuesIn(value, set));
        }

        [Test]
        public void TestMessageUsedInThrownExceptionForValuesIn()
        {
            int value = 4;
            IEnumerable<object> set = new object[] { 1, 2, 3 };
            string expectedException = "test message.";

            var exception = Assert.Throws<ArgumentException>(() => Requires.ValuesIn(value, set, expectedException));

            Assert.AreEqual(expectedException, exception.Message);
        }
    }
}
