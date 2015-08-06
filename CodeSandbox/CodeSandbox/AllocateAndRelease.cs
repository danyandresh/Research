using System;
using System.Diagnostics;
using System.Text;

namespace CodeSandbox
{
    class AllocateAndRelease
    {
        static string indicators = @"/-\|";
        static int currentChar = 0;

        enum AllocationType
        {
            String,
            Array,
            LargeArray,
            MyObject,
        };

        static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit");
            while (!Console.KeyAvailable)
            {
                DisplayProgress();
                DoAnAllocation();
            }
        }

        static void DisplayProgress()
        {
            Console.CursorLeft = 0;
            Console.Write(indicators[currentChar]);
            currentChar += 1;
            currentChar %= indicators.Length;
        }

        static void DoAnAllocation()
        {
            AllocationType type = (AllocationType)rand.Next(0, 4);
            object result = null;
            switch (type)
            {
                case AllocationType.String:
                    result = CreateString();
                    break;
                case AllocationType.Array:
                    result = CreateArray(false);
                    break;
                case AllocationType.LargeArray:
                    // make large objects more rare so GCs aren't dominated by gen2
                    if (rand.Next(0, 100) == 99)
                    {
                        result = CreateArray(true);
                    }
                    break;
                case AllocationType.MyObject:
                    result = CreateMyObject();
                    break;
            }

        }

        static string CreateString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(rand.Next(0, 1000));
            }
            return sb.ToString();
        }

        static int[] CreateArray(bool large)
        {
            int size = large ? 100000 : 1000;
            int[] arr = new int[size];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rand.Next();
            }
            return arr;
        }

        static MyObject CreateMyObject()
        {
            return new MyObject();
        }

        static Process[] CreateProcessList()
        {
            return Process.GetProcesses();
        }
    }
    class MyObject
    {
        int x;
        int y;
        int z;
        int t;
        string label;

        public MyObject()
        {
            x = 0;
            y = 1;
            z = 2;
            t = 3;
            label = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2},{3}): {4}", x, y, z, t, label);
        }

    }

}
