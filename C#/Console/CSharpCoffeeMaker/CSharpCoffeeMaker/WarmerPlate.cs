namespace CSharpCoffeeMaker
{

    public interface IWarmerPlate
    {
        void ToggleWarmer();
        bool IsWarmerEmpty();
    }

    public class WarmerPlate : IWarmerPlate
    {
        private readonly ICoffeeMakerAPI _makerApi;

        public WarmerPlate(ICoffeeMakerAPI makerApi)
        {
            _makerApi = makerApi;
        }

        public void ToggleWarmer()
        {

            var warmerPlateStatus = _makerApi.GetWarmerPlateStatus();
            var newStatus = warmerPlateStatus == WarmerPlateStatus.POT_NOT_EMPTY
                ? WarmerPlateState.WARMER_ON
                : WarmerPlateState.WARMER_OFF;

            _makerApi.SetWarmerState(newStatus);
        }

        public bool IsWarmerEmpty()
        {
            return _makerApi.GetWarmerPlateStatus() == WarmerPlateStatus.WARMER_EMPTY;
        }
    }
}