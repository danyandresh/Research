using System.Collections.Generic;

namespace PrimeNumbers
{
    public interface IDivisorRange
    {
        /// <summary>
        /// A list of potential divisors for a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>A list of potential divisors for a number</returns>
        IEnumerable<int> GetRange(int number);
    }
}
