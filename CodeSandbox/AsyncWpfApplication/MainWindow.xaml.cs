using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AsyncWpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static int _completed;

        private static readonly int _taskCount = 50;

        public MainWindow()
        {
            InitializeComponent();
        }

        public string ButtonMessage
        {
            get
            {
                return string.Format("{0}/{1}", _completed, _taskCount); 
            }
            set
            {
                
            }
        }

        public async void ButtonBase_OnClick(object sender, EventArgs ea)
        {
            var b = sender as Button;
            Console.WriteLine(Thread.CurrentThread.GetApartmentState());
            Console.WriteLine("Current TaskScheduler {0}", TaskScheduler.Current.Id);
            TaskScheduler.FromCurrentSynchronizationContext();
            var sum = await TestSynchronizationContext(b);

            //b.Content = sum;


            while (_completed < _taskCount)
            {
                Task.Yield();
                Task.Delay(100);
            }

            Console.WriteLine(sum);
            ButtonMessage = sum.ToString();
        }

        private async Task<int> TestSynchronizationContext(Button b)
        {
            _completed = 0;
            var semaphore = new ManualResetEventSlim(false);

            var tasks = Enumerable.Range(1, _taskCount)
                .Select(
                    i => Task.Run(
                        () =>
                        {
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

            var sum = 0;
            foreach (var task in tasks)
            {
                var result = await task.ConfigureAwait(continueOnCapturedContext: false);

                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                sum += 1;
                Console.WriteLine(
                    "Item: {0:D2} Launcher thread:{1:D2} Current thread: {2:D2} Result thread: {3:D2} Sum: {4:D2}",
                    result.Item2,
                    launcherThreadId,
                    currentThreadId,
                    result.Item1,
                    sum);

                Interlocked.Increment(ref _completed);
                OnPropertyChanged("ButtonMessage");

            }

            //Give time to the user to play with the application (10 seconds) while the long running task is handled gracefully with async/await
            //If continueOnCapturedContext is set to true, this line would block the UI;
            Thread.Sleep(10 * 1000);

            return sum;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
