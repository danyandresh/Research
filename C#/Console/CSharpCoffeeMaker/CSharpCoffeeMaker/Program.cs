using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpCoffeeMaker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var shouldTerminate = false;

            var firmware = CoffeeMakerAPI.instance;
            var fc = firmware as ICoffeeMakerAPIController;
            CoffeeMakerAPI.instance.SetReliefValveState(ReliefValveState.VALVE_OPEN);
            CoffeeMakerAPI.instance.SetBoilerState(BoilerState.BOILER_OFF);
            CoffeeMakerAPI.instance.SetIndicatorState(IndicatorState.INDICATOR_OFF);
            CoffeeMakerAPI.instance.SetWarmerState(WarmerPlateState.WARMER_OFF);

            var warmer = new WarmerPlate(firmware);
            var boiler = new Boiler(firmware, warmer);
            var brewController = new BrewController(firmware, boiler, warmer);

            var actions = new Dictionary<ConsoleKey, Action>
            {
                {
                    ConsoleKey.End, () => { shouldTerminate = true; }
                },
                {
                    ConsoleKey.P, () =>
                    {
                        fc.WarmerPlateStatus = fc.WarmerPlateStatus.GetNextState();
                    }
                },
                {
                    ConsoleKey.W, () =>
                    {
                        fc.BoilerStatus = fc.BoilerStatus.GetNextState();
                    }
                },
                {
                    ConsoleKey.B, () =>
                    {
                        fc.BrewButtonStatus = fc.BrewButtonStatus.GetNextState();
                    }
                },
                {
                    ConsoleKey.R, () =>
                    {
                    }
                }
            };

            var watcher = new Watcher(boiler, warmer, brewController);
            var checkInterval = TimeSpan.FromMilliseconds(2000);
            watcher.StartWatching(checkInterval);

            LoopThroughActions(() => shouldTerminate, actions, fc, watcher);
        }

        private static void LoopThroughActions(Func<bool> shouldTerminate, IReadOnlyDictionary<ConsoleKey, Action> actions, ICoffeeMakerAPIController firmware, Watcher watcher)
        {
            var iteration = 0;

            while (!shouldTerminate())
            {
                Console.Clear();
                iteration++;
                Console.WriteLine(" Iteration: {0}", iteration);
                PrintFirmwareStatus(firmware);
                PrintOptions(actions.Keys);

                var keyInfo = Console.ReadKey();
                Console.WriteLine(keyInfo.Key.ToString());
                Action action;
                if (actions.TryGetValue(keyInfo.Key, out action))
                {
                    action();
                }
                else
                {
                    Console.WriteLine("No such option");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Bye");
        }

        private static void PrintFirmwareStatus(ICoffeeMakerAPIController firmware)
        {
            var formatPattern = " {0, 20}: {1, -20}";
            Console.WriteLine(formatPattern, "WarmerPlateStatus", firmware.WarmerPlateStatus);
            Console.WriteLine(formatPattern, "BrewButtonStatus", firmware.BrewButtonStatus);
            Console.WriteLine(formatPattern, "BoilerStatus", firmware.BoilerStatus);
            Console.WriteLine(formatPattern, "WarmerPlateState", firmware.WarmerPlateState);
            Console.WriteLine(formatPattern, "BoilerState", firmware.BoilerState);
            
            Console.WriteLine(formatPattern, "ReliefValveState", firmware.ReliefValveState);

            Console.WriteLine(" Machine status");
            Console.WriteLine(formatPattern, "IndicatorState", firmware.IndicatorState);
        }

        private static void PrintOptions(IEnumerable<ConsoleKey> keys)
        {
            Console.WriteLine();
            Console.Write("Options: ");
            var options = keys.Select(k => k.ToString())
                .Aggregate((k, c) => string.Format("{0}, {1}", k, c));

            Console.Write("{0}; ", options);
        }
    }
}