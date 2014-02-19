using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeNumbers
{
    public class DivisorsRange : IDivisorRange
    {
        public IEnumerable<int> GetRange(int number)
        {
            // Build the list of divisors starting from 2 up to square root of the number
            return Enumerable.Range(2, (int)Math.Sqrt(number) - 1);
        }
    }
}
