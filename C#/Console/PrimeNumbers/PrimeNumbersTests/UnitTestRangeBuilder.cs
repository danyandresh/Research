using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeNumbers;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using System.Collections.Concurrent;

namespace PrimeNumbersTests
{
    [TestClass]
    public class UnitTestRangeBuilder : TestContext
    {

        [TestMethod]
        public void TestDivisorsRangeGoesFromAToB()
        {
            var numberA = randomizer.Next(2, 200);
            var numberB = randomizer.Next(201, 2000);

            var primeChecker = new Mock<IPrimeNumberChecker>();
            primeChecker.Setup(p => p.IsPrime(It.IsAny<int>())).Returns(true);
            IRangeBuilder numbersRange = new RangeBuilder(primeChecker.Object);

            //Enforce built range to be parallel query
            var range = numbersRange.GetRangeOfPrimes(numberA, numberB);
            Assert.AreEqual(numberA, range.Min(), "The divisors range should start from numberA");
            Assert.AreEqual(numberB, range.Max(), "The divisors range should start from numberA");
        }

        [TestMethod]
        public void TestDivisorsRangeAcceptsAGreaterThanB()
        {
            var numberA = randomizer.Next(2, 200);
            var numberB = randomizer.Next(201, 2000);

            var primeChecker = new Mock<IPrimeNumberChecker>();
            primeChecker.Setup(p => p.IsPrime(It.IsAny<int>())).Returns(true);
            IRangeBuilder numbersRange = new RangeBuilder(primeChecker.Object);

            var range = numbersRange.GetRangeOfPrimes(numberB, numberA);
            Assert.AreEqual(numberA, range.Min(), "The divisors range should start from numberA");
            Assert.AreEqual(numberB, range.Max(), "The divisors range should start from numberA");
        }

        [TestMethod]
        public void TestDivisorsRangeBuildsTheFullRangeFromAToB()
        {
            var numberA = randomizer.Next(2, 200);
            var numberB = randomizer.Next(201, 2000);

            var primeChecker = new Mock<IPrimeNumberChecker>();
            primeChecker.Setup(p => p.IsPrime(It.IsAny<int>())).Returns(true);
            IRangeBuilder numbersRange = new RangeBuilder(primeChecker.Object);

            var range = numbersRange.GetRangeOfPrimes(numberA, numberB);

            var expectedNumbersInRange = ParallelEnumerable.Range(numberA, numberB - numberA + 1);

            Assert.IsTrue(expectedNumbersInRange.All(n => range.Contains(n)));
        }

        [TestMethod]
        public void TestDivisorsRangeBuildsEachNumberInRangeIsPrime()
        {
            var numberA = randomizer.Next(2, 200);
            var numberB = randomizer.Next(201, 2000);

            var divisorsRange = new DivisorsRange();
            var primeChecker = new PrimeNumbers.PrimeNumberChecker(divisorsRange);

            IRangeBuilder numbersRange = new RangeBuilder(primeChecker);

            var range = numbersRange.GetRangeOfPrimes(numberA, numberB);

            foreach (var n in range)
            {
                Assert.IsTrue(primeChecker.IsPrime(n), string.Format("{0} is not prime", n));
            }
        }
    }
}
