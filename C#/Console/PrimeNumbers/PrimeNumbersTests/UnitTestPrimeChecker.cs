using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeNumbers;
using System.Linq;
using Moq;

namespace PrimeNumbersTests
{
    [TestClass]
    public class UnitTestPrimeChecker : TestContext
    {
        [TestMethod]
        public void TestIsPrime()
        {
            var number = 11;

            var divisorsRange = new Mock<IDivisorRange>();
            divisorsRange.Setup(d => d.GetRange(It.IsAny<int>())).Returns<int>((n) => ParallelEnumerable.Range(2, (int)Math.Sqrt(n)));

            var checker = new PrimeNumberChecker(divisorsRange.Object);

            Assert.IsTrue(checker.IsPrime(number));
        }

        [TestMethod]
        public void TestIsNotPrimeWhenDivisorsRangeContainsOnlyItself()
        {
            var number = 11;

            var divisorsRange = new Mock<IDivisorRange>();
            divisorsRange.Setup(d => d.GetRange(It.IsAny<int>())).Returns<int>((n) => ParallelEnumerable.Range(n, 1));

            var checker = new PrimeNumberChecker(divisorsRange.Object);

            Assert.IsFalse(checker.IsPrime(number));
        }

        [TestMethod]
        public void TestIsNotPrime()
        {
            var randomizer = new Random();

            // Check a product of two random numbers as not prime
            var number = randomizer.Next(2, 200) * randomizer.Next(2, 200);
            var divisorsRange = new DivisorsRange();
            var checker = new PrimeNumberChecker(divisorsRange);

            Assert.IsFalse(checker.IsPrime(number));
        }
    }
}
