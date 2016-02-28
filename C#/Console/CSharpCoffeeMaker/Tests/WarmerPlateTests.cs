using CSharpCoffeeMaker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class WarmerPlateTests : TestWithFirmware
    {
        [TestMethod]
        public void TestWarmerReady()
        {
            var warmerPlate = new WarmerPlate(_firmwareApi);
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            Assert.IsFalse(warmerPlate.IsWarmerEmpty());
        }

        [TestMethod]
        public void TestWarmerReadyEmptyWarmer()
        {
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            Assert.IsTrue(warmerPlate.IsWarmerEmpty());
        }

        [TestMethod]
        public void TestWarmerCanTurnOnWhenPotNotEmpty()
        {
            var warmerPlate = new WarmerPlate(_firmwareApi);
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_NOT_EMPTY;

            warmerPlate.ToggleWarmer();
            Assert.AreEqual(WarmerPlateState.WARMER_ON, _firmwareApiController.WarmerPlateState);
        }

        [TestMethod]
        public void TestWarmerPlateCantTurnOnIfWarmerIsEmpty()
        {
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            warmerPlate.ToggleWarmer();
            Assert.AreEqual(WarmerPlateState.WARMER_OFF, _firmwareApiController.WarmerPlateState);

        }

        [TestMethod]
        public void TestWarmerPlateCantTurnOnIfPotIsEmpty()
        {
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            warmerPlate.ToggleWarmer();
            Assert.AreEqual(WarmerPlateState.WARMER_OFF, _firmwareApiController.WarmerPlateState);
        }

        [TestMethod]
        public void TestWarmerPlateCanTurnOnIfPotIsNotEmpty()
        {
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_NOT_EMPTY;
            _firmwareApiController.WarmerPlateState = WarmerPlateState.WARMER_OFF;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            warmerPlate.ToggleWarmer();

            Assert.AreEqual(WarmerPlateState.WARMER_ON, _firmwareApiController.WarmerPlateState);
        }
    }
}