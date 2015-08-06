using System;
using System.Runtime.CompilerServices;

namespace CodeSandbox
{
    class JitCall
    {
        static void Main(string[] args)
        {
            int val = A();
            int val2 = A();

            Console.WriteLine(val + val2);
        }

        //[MethodImpl(MethodImplOptions.NoInlining)]
        static int A()
        {
            return 42;
        }
    }
}
