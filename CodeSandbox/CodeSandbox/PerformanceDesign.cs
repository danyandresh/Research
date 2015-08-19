using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeSandbox
{
    internal class PerformanceDesign
    {
        public static void Main(string[] args)
        {
            var personCount = (int)Math.Pow(10, 7);
            var names = Enumerable.Range(0, personCount)
                .Select(i => "Person_" + i.ToString("N7"))
                .ToArray();
            var actualMemory = Process.GetCurrentProcess()
                .WorkingSet64;
            Console.WriteLine("Initial working set {0:N}", TransformSize(actualMemory));

            MeasureInitialization<PersonStruct>(names);
            MeasureInitialization<PersonClass>(names);
        }

        static void MeasureInitialization<T>(string[] names) where T : IPerson
        {
            var counter = Stopwatch.StartNew();

            var result = names.Select(
                n =>
                {
                    var t = Activator.CreateInstance<T>();
                    t.Name = n;

                    return t;
                })
                .ToArray();

            counter.Stop();
            var actualMemory = Process.GetCurrentProcess()
                .WorkingSet64;
            Console.WriteLine(
                "{3} testing: working set: {0:N}; time spent: {1:N0}ms; elements count {2:N}",
                TransformSize(actualMemory),
                counter.ElapsedMilliseconds,
                result.Length,
                typeof(T).IsClass
                    ? "Class "
                    : "Struct");
            //GC.Collect(2);
        }

        private static string TransformSize(long byteSize)
        {
            var supportedFormats = new SortedDictionary<double, string>
            {
                {Math.Pow(2, 0), "B"},
                {Math.Pow(2, 10), "KB"},
                {Math.Pow(2, 20), "MB"},
                {Math.Pow(2, 30), "GB"},
                {Math.Pow(2, 40), "TB"}
            };

            var sizeConvertor = supportedFormats.LastOrDefault(x => x.Key < byteSize);

            if (sizeConvertor.Equals(default(KeyValuePair<double, string>)))
            {
                sizeConvertor = supportedFormats.First();
            }

            var newSize = byteSize / sizeConvertor.Key;

            var result = string.Format("{0:0.##} {1}", newSize, sizeConvertor.Value);

            return result;
        }
    }

    interface IPerson
    {
        string Name { get; set; }
    }

    struct PersonStruct : IPerson
    {
        public string Name { get; set; }
    }

    class PersonClass : IPerson
    {
        public string Name { get; set; }
    }
}