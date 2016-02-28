namespace CSharpCoffeeMaker
{
    public interface IBrewController
    {
        void StartBrewing();
        void StopBrewing();
    }

    public class BrewController : IBrewController
    {
        private readonly ICoffeeMakerAPI _makerApi;
        private readonly IBoiler _boiler;
        private readonly IWarmerPlate _warmerPlate;

        public BrewController(ICoffeeMakerAPI makerApi, IBoiler boiler, IWarmerPlate warmerPlate)
        {
            _makerApi = makerApi;
            _boiler = boiler;
            _warmerPlate = warmerPlate;
        }

        public bool IsBrewButtonPushed()
        {
            return _makerApi.GetBrewButtonStatus() == BrewButtonStatus.BREW_BUTTON_PUSHED;
        }

        public void StartBrewing()
        {
            _makerApi.SetIndicatorState(IndicatorState.INDICATOR_OFF);

            if (!_boiler.IsReady())
            {
                throw new BoilerNotReadyException();
            }

            _boiler.TurnOn();

            if (_warmerPlate.IsWarmerEmpty())
            {
                throw new WarmerPlateNotReadyException();
            }

            _warmerPlate.ToggleWarmer();
        }

        public void StopBrewing()
        {
            _makerApi.SetIndicatorState(IndicatorState.INDICATOR_ON);
            _boiler.TurnOff();
        }
    }
}