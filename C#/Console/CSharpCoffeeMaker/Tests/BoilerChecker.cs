using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpCoffeeMaker;

namespace Tests
{
    [TestClass]
    public class BoilerTests : TestWithFirmware
    {
        [TestMethod]
        public void TestReadyBoiler()
        {
            var boiler = new Boiler(_firmwareApi, null);
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;

            Assert.IsTrue(boiler.IsReady());
        }

        [TestMethod]
        public void TestNotReadyBoiler()
        {
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_EMPTY;
            var boiler = new Boiler(_firmwareApi, null);

            Assert.IsFalse(boiler.IsReady());
        }

        [TestMethod]
        public void TestTurnOnBoiler()
        {
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            var boiler = new Boiler(_firmwareApi, warmerPlate);
            boiler.TurnOn();

            Assert.AreEqual(BoilerState.BOILER_ON, _firmwareApiController.BoilerState);
        }

        [TestMethod]
        public void TestTurnOffBoiler()
        {
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            var boiler = new Boiler(_firmwareApi, warmerPlate);
            boiler.TurnOn();
            boiler.TurnOff();

            Assert.AreEqual(BoilerState.BOILER_OFF, _firmwareApiController.BoilerState);
        }

        [TestMethod]
        public void TestTurnOffBoilerOpensReliefValve()
        {
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
            var warmerPlate = new WarmerPlate(_firmwareApi);

            var boiler = new Boiler(_firmwareApi, warmerPlate);
            boiler.TurnOn();
            boiler.TurnOff();

            Assert.AreEqual(ReliefValveState.VALVE_OPEN, _firmwareApiController.ReliefValveState);
        }

        [TestMethod]
        [ExpectedException(typeof(BoilerEmptyException), "Can't turn on boiler if there is no water")]
        public void TestCantTurnOnBoilerIfNoWater()
        {
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_EMPTY;
            var boiler = new Boiler(_firmwareApi, null);
            boiler.TurnOn();

            Assert.AreEqual(BoilerState.BOILER_OFF, _firmwareApiController.BoilerState);
        }

        [TestMethod]
        public void TestTurningOnBoilerWhenWarmerEmptyOpensReliefValve()
        {
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;

            var warmerPlate = new WarmerPlate(_firmwareApi);

            var boiler = new Boiler(_firmwareApi, warmerPlate);
            boiler.TurnOn();

            Assert.AreEqual(ReliefValveState.VALVE_OPEN, _firmwareApiController.ReliefValveState);
        }

        [TestMethod]
        public void TestTurningOnBoilerWhenWarmerNotEmptyClosesReliefValve()
        {
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var warmerPlate = new WarmerPlate(_firmwareApi);

            var boiler = new Boiler(_firmwareApi, warmerPlate);
            boiler.TurnOn();

            Assert.AreEqual(ReliefValveState.VALVE_CLOSED, _firmwareApiController.ReliefValveState);
        }
    }
}
