using System.Diagnostics.Tracing;

namespace CodeSandbox
{
    internal class Etw
    {
        static void Main(string[] args)
        {
            Events.Write.ProcessingStart();
            Events.Write.FoundPrime(3);
            Events.Write.ProcessingFinish();
        }

        [EventSource(Name = "EtlDemo")]
        sealed class Events : EventSource
        {
            public static readonly Events Write = new Events();

            public class Keywords
            {
                public const EventKeywords General = (EventKeywords)1;
                public const EventKeywords PrimeOutput = (EventKeywords)2;
            }

            internal const int ProcessingStartId = 1;
            internal const int ProcessingFinishId = 2;
            internal const int FoundPrimeId = 3;

            [Event(ProcessingStartId, Level = EventLevel.Informational, Keywords = Keywords.General)]
            public void ProcessingStart()
            {
                if (IsEnabled())
                {
                    WriteEvent(ProcessingStartId);
                }
            }

            [Event(ProcessingFinishId, Level = EventLevel.Informational, Keywords = Keywords.PrimeOutput)]
            public void ProcessingFinish()
            {
                if (IsEnabled())
                {
                    WriteEvent(ProcessingFinishId);
                }
            }

            [Event(FoundPrimeId, Level = EventLevel.Informational, Keywords = Keywords.PrimeOutput)]
            public void FoundPrime(long primeNumber)
            {
                if (IsEnabled())
                {
                    WriteEvent(FoundPrimeId, primeNumber);
                }
            }

            [Event(5, Level = EventLevel.Informational, Keywords = Keywords.General)]
            public void Error(string message)
            {
                if (IsEnabled())
                {
                    WriteEvent(5, message ?? string.Empty);
                }
            }
        }
    }
}