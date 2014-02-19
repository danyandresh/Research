using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PrimeNumbersTests
{
    public class TestContext
    {
        protected Random randomizer;

        [TestInitialize]
        public void Initialization()
        {
            randomizer = new Random();
        }

        protected int GetATestNumber()
        {
            return randomizer.Next(2, 200);
        }
    }
}
