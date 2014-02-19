using System.Linq;

namespace PrimeNumbers
{
    public class PrimeNumberChecker : IPrimeNumberChecker
    {
        private IDivisorRange divisorRange;

        public PrimeNumberChecker(IDivisorRange divisorRange)
        {
            this.divisorRange = divisorRange;
        }
        
        public bool IsPrime(int number)
        {
            // A number is prime if it can't be exactly divided to any of his his supposed divisors
            return divisorRange.GetRange(number).All(n => number % n != 0);
        }
    }
}
