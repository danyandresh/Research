using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace PrimeNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b;
            if(args.Length!=2||!int.TryParse(args[0],out a)||!int.TryParse(args[1],out b))
            {
                Console.WriteLine("{0} are not valid arguments; please provide two integers", string.Join(" ", args));
                return;
            }

            // This program takes advantage of the Task Parallel Library and usees all cores on the machine - this way is ~ 2/5 faster on a quad core machine

            var divisorsRange = new DivisorsRange();
            var primeChecker = new PrimeNumberChecker(divisorsRange);
            var rangeBuilder = new RangeBuilder(primeChecker);

            var counter = new Stopwatch();
            counter.Start();
            var primes = rangeBuilder.GetRangeOfPrimes(a, b).ToList();
            counter.Stop();
            Console.WriteLine("It took {0} to calculate", counter.Elapsed);
            foreach (var n in primes)
            {
                Console.Write("{0}, ", n);
            }
        }

        static int ReadNumber()
        {
            Console.Write("Give a number: ");
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("That was not an int number");
                Console.Write("Give a number: ");
            }

            return result;
        }
    }
}
