using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeNumbers;
using System.Threading.Tasks;
using System.Linq;
using Moq;

namespace PrimeNumbersTests
{
    [TestClass]
    public class UnitTestDivisorRange : TestContext
    {
        [TestMethod]
        public void TestDivisorsRangeGoesFrom2()
        {
            var number = GetATestNumber();

            IDivisorRange divisorsRange = new DivisorsRange();

            var range = divisorsRange.GetRange(number);
            Assert.AreEqual(2, range.Min(), "The divisors range should start from 2");
        }

        [TestMethod]
        public void TestDivisorsRangeGoesToSqrt()
        {
            var number = GetATestNumber();

            IDivisorRange divisorsRange = new DivisorsRange();

            var range = divisorsRange.GetRange(number);
            Assert.AreEqual((int)Math.Sqrt(number), range.Max(), "The divisors range should start from 2");
        }

        [TestMethod]
        public void TestDivisorsRangeBuildsTheFullRange()
        {
            var number = GetATestNumber();

            IDivisorRange divisorsRange = new DivisorsRange();

            var range = divisorsRange.GetRange(number);

            //TODO Change this to only select the prime number in the range, so to optimize the algorithm
            var elementCount = (int)Math.Sqrt(number) - 1; // take out element 1
            var expectedNumbersInRange = ParallelEnumerable.Range(2, elementCount);

            Assert.IsTrue(expectedNumbersInRange.All(n => range.Contains(n)));
        }
    }
}
