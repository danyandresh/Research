using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace CodeSandbox
{
    internal class Etw
    {
        private static void Main(string[] args)
        {
            var consoleListener = new ConsoleListener(
                new[]
                {
                    new BaseListener.SourceConfig
                    {
                        Name = "EtlDemo",
                        Level = EventLevel.Informational,
                        Keywords = Events.Keywords.General | Events.Keywords.PrimeOutput
                    }
                });

            var start = (long)Math.Pow(10, 6);
            var end = start + (long)Math.Pow(10, 4);

            Events.Write.ProcessingStart();
            for (var i = start; i < end; i++)
            {
                if (IsPrime(i))
                {
                    Events.Write.FoundPrime(i);
                }
            }

            Events.Write.FoundPrime(3);
            Events.Write.ProcessingFinish();

            consoleListener.Dispose();
        }

        private static bool IsPrime(long number)
        {
            var squareRoot = (long)Math.Floor(Math.Sqrt(number));
            for (long i = 2; i <= squareRoot; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        [EventSource(Name = "EtlDemo")]
        sealed class Events : EventSource
        {
            internal const int ProcessingStartId = 1;
            internal const int ProcessingFinishId = 2;
            internal const int FoundPrimeId = 3;
            public static readonly Events Write = new Events();

            [Event(ProcessingStartId, Level = EventLevel.Informational, Keywords = Keywords.General)]
            public void ProcessingStart()
            {
                if (IsEnabled())
                {
                    WriteEvent(ProcessingStartId);
                }
            }

            [Event(ProcessingFinishId, Level = EventLevel.Informational, Keywords = Keywords.General)]
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

            public class Keywords
            {
                public const EventKeywords General = (EventKeywords)1;
                public const EventKeywords PrimeOutput = (EventKeywords)2;
            }
        }

        abstract class BaseListener : EventListener
        {
            private readonly List<SourceConfig> configs = new List<SourceConfig>();

            protected BaseListener(IEnumerable<SourceConfig> sources)
            {
                configs.AddRange(sources);

                foreach (var sourceConfig in configs)
                {
                    var eventSource = FindEventSource(sourceConfig.Name);

                    if (eventSource == null)
                    {
                        continue;
                    }

                    EnableEvents(eventSource, sourceConfig.Level, sourceConfig.Keywords);
                }
            }

            protected override void OnEventSourceCreated(EventSource eventSource)
            {
                base.OnEventSourceCreated(eventSource);

                foreach (
                    var sourceConfig in
                        configs.Where(sourceConfig => string.Equals(sourceConfig.Name, eventSource.Name)))
                {
                    EnableEvents(eventSource, sourceConfig.Level, sourceConfig.Keywords);
                }
            }

            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                WriteEvent(eventData);
            }

            protected abstract void WriteEvent(EventWrittenEventArgs eventData);

            private static EventSource FindEventSource(string name)
            {
                var result = EventSource.GetSources()
                    .FirstOrDefault(eventSource => string.Equals(eventSource.Name, name));

                return result;
            }

            public class SourceConfig
            {
                public string Name { get; set; }
                public EventLevel Level { get; set; }
                public EventKeywords Keywords { get; set; }
            }
        }

        class ConsoleListener : BaseListener
        {
            private readonly IReadOnlyDictionary<int, Func<EventWrittenEventArgs, string>> eventWriteFormaters = new Dictionary
                <int, Func<EventWrittenEventArgs, string>>
            {
                {Events.ProcessingStartId, e => string.Format("ProcessingStart ({0})", e.EventId)},
                {Events.ProcessingFinishId, e => string.Format("ProcessingFinish ({0})", e.EventId)},
                {Events.FoundPrimeId, e => string.Format("FoundPrime ({0}):{1}", e.EventId, e.Payload[0])}
            };

            public ConsoleListener(IEnumerable<SourceConfig> sources)
                : base(sources)
            {
            }

            protected override void WriteEvent(EventWrittenEventArgs eventData)
            {
                Func<EventWrittenEventArgs, string> outputStringFormater;
                if (!eventWriteFormaters.TryGetValue(eventData.EventId, out outputStringFormater))
                {
                    throw new InvalidOperationException("Unknown event: " + eventData.EventId);
                }

                var outputMessage = outputStringFormater(eventData);

                Console.WriteLine(outputMessage);
            }
        }
    }
}