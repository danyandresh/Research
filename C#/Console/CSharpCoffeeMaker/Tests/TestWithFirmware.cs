using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public class TestWithFirmware
    {
        protected ICoffeeMakerAPI _firmwareApi;
        protected ICoffeeMakerAPIController _firmwareApiController;

        [TestInitialize]
        public void ApiFirmware()
        {
            _firmwareApi = CoffeeMakerAPI.instance;
            _firmwareApiController = (ICoffeeMakerAPIController)CoffeeMakerAPI.instance;
        }
    }
}