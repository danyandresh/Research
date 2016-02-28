namespace CSharpCoffeeMaker
{
    public interface IBoiler
    {
        bool IsReady();

        void TurnOn();
        void TurnOff();
    }

    public class Boiler : IBoiler
    {
        private readonly ICoffeeMakerAPI _makerApi;
        private readonly IWarmerPlate _warmerPlate;

        public Boiler(ICoffeeMakerAPI makerApi, IWarmerPlate warmerPlate)
        {
            _makerApi = makerApi;
            _warmerPlate = warmerPlate;
        }

        public bool IsReady()
        {
            return _makerApi.GetBoilerStatus() == BoilerStatus.BOILER_NOT_EMPTY;
        }

        public void TurnOn()
        {
            if (!IsReady())
            {
                throw new BoilerEmptyException();
            }

            var toggleReliefState = _warmerPlate.IsWarmerEmpty()
                ? ReliefValveState.VALVE_OPEN
                : ReliefValveState.VALVE_CLOSED;

            _makerApi.SetReliefValveState(toggleReliefState);

            _makerApi.SetBoilerState(BoilerState.BOILER_ON);
        }

        public void TurnOff()
        {
            _makerApi.SetBoilerState(BoilerState.BOILER_OFF);
            _makerApi.SetReliefValveState(ReliefValveState.VALVE_OPEN);
        }
    }
}