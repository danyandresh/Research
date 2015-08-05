using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter_02_GarbageCollector
{
    class Program
    {
        const int ArraySize = 1000;

        static object[] staticArray = new object[ArraySize];

        static void Main(string[] args)
        {
            object[] localArray = new object[ArraySize];

            var rand = new Random();
            Console.WriteLine("Allocating memory");
            for (int i = 0; i < ArraySize; i++)
            {
                Console.Write("Line {0}", i);
                Console.CursorLeft = 0;
                staticArray[i] = GetNewObject(rand.Next(0, 4));
                localArray[i] = GetNewObject(rand.Next(0, 4));
            }

            Console.WriteLine("Use PerfView to examine the heap now. Press any key to exit...");
            Console.ReadKey();

            Console.WriteLine(staticArray.Length);
            Console.WriteLine(localArray.Length);
        }

        private static Base GetNewObject(int type)
        {
            Base obj = null;
            switch (type)
            {
                case 0: obj = new A(); break;
                case 1: obj = new B(); break;
                case 2: obj = new C(); break;
                case 3: obj = new D(); break;
            }

            return obj;
        }


    }

    public class Base
    {
        private byte[] memory;
        protected Base(int size) { this.memory = new byte[size]; }
    }

    public class A : Base { public A() : base(1000) { } }
    public class B : Base { public B() : base(10000) { } }
    public class C : Base { public C() : base(1000000) { } }
    public class D : Base { public D() : base(10000000) { } }
}
