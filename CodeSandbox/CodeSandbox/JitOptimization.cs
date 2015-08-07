using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading.Tasks;
namespace CodeSandbox
{
    class JitOptimization
    {
        static void Main(string[] args)
        {
            // This folder must already exist 
            ProfileOptimization.SetProfileRoot(@"C:\CodeSandboxProfile");
            ProfileOptimization.StartProfile("default");

            const int testsCount = int.MaxValue / 10;

            var instance=new JitOptimization();
            var taskWithInlining = Task.Run(() =>
            {
                var counter = Stopwatch.StartNew();
                var cumulated = int.MinValue;

                for (int i = 0; i < testsCount; i++)
                {
                    var value = instance.DummyIntValueWithInlineCapability();
                    cumulated += value;
                }

                counter.Stop();

                return Tuple.Create(counter, cumulated);
            });

            var taskNoInlining = Task.Run(() =>
            {
                var counter = Stopwatch.StartNew();
                var cumulated = int.MinValue;

                for (int i = 0; i < testsCount; i++)
                {
                    var value = instance.DummyIntValueNoInlining();
                    cumulated += value;
                }

                counter.Stop();

                return Tuple.Create(counter, cumulated);
            });

            Task.WaitAll(taskWithInlining, taskNoInlining);
            
            Console.WriteLine("With inlining {0}", taskWithInlining.Result.Item1.Elapsed);  
          
            Console.WriteLine("Without inlining {0}", taskNoInlining.Result.Item1.Elapsed);
        }

        public int DummyIntValueWithInlineCapability()
        {
            return 1;
        }

        public virtual dynamic DummyIntValueNoInlining()
        {
            try
            {
                for (int i = 0; i < 0; i++)
                { }

                return 1;
            }
            catch (Exception ex)
            {
                // methods containing loops, exception handling, dynamic, linq expressions or are marked as virtual are not inlined;
            }

            return 0;
        }
    }
}
