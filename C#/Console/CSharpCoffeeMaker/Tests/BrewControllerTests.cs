using CSharpCoffeeMaker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class BrewControllerTests : TestWithFirmware
    {
        [TestMethod]
        public void TestBrewButtonNotPushed()
        {
            var machine = new BrewController(_firmwareApi, null, null);

            Assert.IsFalse(machine.IsBrewButtonPushed());
        }

        [TestMethod]
        public void TestBrewButtonPushed()
        {
            var machine = new BrewController(_firmwareApi, null, null);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;

            Assert.IsTrue(machine.IsBrewButtonPushed());
        }

        [TestMethod]
        public void TestBrewButtonTransientPushState()
        {
            var machine = new BrewController(_firmwareApi, null, null);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;

            Assert.IsTrue(machine.IsBrewButtonPushed());
            Assert.IsFalse(machine.IsBrewButtonPushed());
        }

        [TestMethod]
        public void TestBrewStartsTurnIndicatorOff()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            brewController.StartBrewing();

            Assert.AreEqual(IndicatorState.INDICATOR_OFF, _firmwareApiController.IndicatorState);
        }

        [TestMethod]
        public void TestBrewStopsTurnIndicatorOn()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            brewController.StartBrewing();
            brewController.StopBrewing();

            Assert.AreEqual(IndicatorState.INDICATOR_ON, _firmwareApiController.IndicatorState);
        }

        [TestMethod]
        public void TestBrewStopsTurnBoilerOff()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            brewController.StartBrewing();
            brewController.StopBrewing();

            Assert.AreEqual(BoilerState.BOILER_OFF, _firmwareApiController.BoilerState);
        }

        [TestMethod]
        public void TestBrewStopsTurnOpenReliefValve()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            brewController.StopBrewing();

            Assert.AreEqual(ReliefValveState.VALVE_OPEN, _firmwareApiController.ReliefValveState);
        }

        [TestMethod]
        public void TestBrewStartsTurnBoilerOn()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            brewController.StartBrewing();

            Assert.AreEqual(BoilerState.BOILER_ON, _firmwareApiController.BoilerState);
        }

        [TestMethod]
        public void TestBrewStartTurnsWarmerOn()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
            _firmwareApiController.WarmerPlateState = WarmerPlateState.WARMER_OFF;

            brewController.StartBrewing();

            Assert.AreEqual(WarmerPlateState.WARMER_ON, _firmwareApiController.WarmerPlateState);
        }

        [TestMethod]
        public void TestBrewStopKeepsWarmerOn()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
            _firmwareApiController.WarmerPlateState = WarmerPlateState.WARMER_OFF;

            brewController.StartBrewing();

            Assert.AreEqual(WarmerPlateState.WARMER_ON, _firmwareApiController.WarmerPlateState);
        }

        [TestMethod]
        [ExpectedException(typeof(BoilerNotReadyException), "Can't start brewing if there is no water")]
        public void TestBrewCantStartIfNoWater()
        {
            var boiler = new Boiler(_firmwareApi, null);
            var brewController = new BrewController(_firmwareApi, boiler, null);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_EMPTY;

            brewController.StartBrewing();

            Assert.AreEqual(IndicatorState.INDICATOR_OFF, _firmwareApiController.IndicatorState);
        }

        [TestMethod]
        [ExpectedException(typeof(WarmerPlateNotReadyException), "Can't start brewing if there is no water")]
        public void TestBrewCantStartIfNoPot()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;

            brewController.StartBrewing();

            Assert.AreEqual(IndicatorState.INDICATOR_OFF, _firmwareApiController.IndicatorState);
        }
    }
}