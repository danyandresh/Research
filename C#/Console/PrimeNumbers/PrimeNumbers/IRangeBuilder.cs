using System.Collections.Generic;

namespace PrimeNumbers
{
    public interface IRangeBuilder
    {
        IEnumerable<int> GetRangeOfPrimes(int numberA, int numberB);
    }
}
