using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeSandbox
{
    internal class Program
    {
        private static int _completed;

        private static readonly int _threadCount = 50;

        [STAThread]
        private static void Main(string[] args)
        {
            LaunchSynchronizationContext();
        }

        private static void LaunchSynchronizationContext()
        {
            //Task.Run(
            //    () =>
            //    {
            Console.WriteLine(Thread.CurrentThread.GetApartmentState());
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            Console.WriteLine("Current TaskScheduler {0}", TaskScheduler.Current.Id);
            TaskScheduler.FromCurrentSynchronizationContext();
            TestSynchronizationContext().Wait();

            while (_completed < _threadCount)
            {
                Task.Yield();
                Task.Delay(100);
            }
            //}).Wait();
        }

        private static async Task TestSynchronizationContext()
        {
            _completed = 0;
            var semaphore = new ManualResetEventSlim(false);

            var tasks = Enumerable.Range(1, _threadCount)
                .Select(
                    i => Task.Run(
                        () =>
                        {
                            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                            semaphore.Wait();

                            return Tuple.Create(currentThreadId, i);
                        }));

            Task.Run(
                () =>
                {
                    Task.Delay(1000);
                    semaphore.Set();
                });

            var launcherThreadId = Thread.CurrentThread.ManagedThreadId;

            foreach (var task in tasks)
            {
                var result = await task;

                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine(
                    "Item: {0:D2} Launcher thread:{1:D2} Current thread: {2:D2} Result thread: {3:D2}",
                    result.Item2,
                    launcherThreadId,
                    currentThreadId,
                    result.Item1);

                Interlocked.Increment(ref _completed);
            }
        }
    }
}