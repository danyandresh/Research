using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PrimeNumbers
{
    public class RangeBuilder : IRangeBuilder
    {
        private IPrimeNumberChecker primeChecker;

        public RangeBuilder(IPrimeNumberChecker primeChecker)
        {
            this.primeChecker = primeChecker;
        }

        public IEnumerable<int> GetRangeOfPrimes(int numberA, int numberB)
        {
            var number1 = Math.Min(numberA, numberB);
            var number2 = Math.Max(numberA, numberB);
            
            // Use TPL and execute the search for prime numbers leveraging all computation power available

            // build a range of numbers to be checked for prime
            var range = Enumerable.Range(number1, number2 - number1 + 1).ToArray();

            // feed each thread with a chunk from range, force this with partitions
            var chunkedPartitionedRange = Partitioner.Create(range, true).AsParallel();

            // do the work of detecting which numbers from partition are prime
            return chunkedPartitionedRange.Where(n => primeChecker.IsPrime(n));
        }
    }
}
