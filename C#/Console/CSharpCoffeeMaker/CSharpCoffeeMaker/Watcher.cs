using System;
using System.Threading.Tasks;

namespace CSharpCoffeeMaker
{
    public class Watcher
    {
        private readonly IBoiler _boiler;
        private readonly IWarmerPlate _warmerPlate;
        private readonly IBrewController _brewController;
        private Task _watcher;

        public Watcher(IBoiler boiler, IWarmerPlate warmerPlate, IBrewController brewController)
        {
            _boiler = boiler;
            _warmerPlate = warmerPlate;
            _brewController = brewController;
        }

        public void StartWatching(TimeSpan checkInterval)
        {
            _watcher = Task.Run(
                () =>
                {
                    while (true)
                    {
                        TriggerBrewing();
                        Task.Delay(checkInterval)
                            .Wait();
                    }
                });
        }

        public void TriggerBrewing()
        {
            if (!_boiler.IsReady() || _warmerPlate.IsWarmerEmpty())
            {
                _brewController.StopBrewing();
            }
            else
            {
                _brewController.StartBrewing();
            }
        }

        public void Wait()
        {
            _watcher.Wait();
        }
    }
}