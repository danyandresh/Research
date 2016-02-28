using System;
using System.Threading.Tasks;
using CSharpCoffeeMaker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class WatcherTests : TestWithFirmware
    {
        [TestMethod]
        public void TestReliefValveClosedWhenReadyForBrewing()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            brewController.StartBrewing();

            Assert.AreEqual(ReliefValveState.VALVE_CLOSED, _firmwareApiController.ReliefValveState);
        }

        [TestMethod]
        public void TestBrewingShouldStartOnBrewButtonPressed()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, null);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var watcher = new Watcher(boiler, warmer, brewController);
            var checkInterval = TimeSpan.FromMilliseconds(1);
            watcher.StartWatching(checkInterval);

            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            Task.Delay(checkInterval.Add(TimeSpan.FromSeconds(1)));

            Assert.AreEqual(ReliefValveState.VALVE_CLOSED, _firmwareApiController.ReliefValveState);
        }

        [TestMethod]
        public void TestBrewingStartTurnsOffIndicator()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.IndicatorState = IndicatorState.INDICATOR_ON;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var watcher = new Watcher(boiler, warmer, brewController);
            var checkInterval = TimeSpan.FromMilliseconds(1);
            watcher.StartWatching(checkInterval);

            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            Task.Delay(checkInterval.Add(TimeSpan.FromSeconds(1))).Wait();

            Assert.AreEqual(IndicatorState.INDICATOR_OFF, _firmwareApiController.IndicatorState);
        }

        [TestMethod]
        public void TestWatchingDoesThrowOnEmptyBoiler()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, null);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var watcher = new Watcher(boiler, warmer, brewController);
            var checkInterval = TimeSpan.FromMilliseconds(1);
            watcher.StartWatching(checkInterval);

            _firmwareApiController.BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_PUSHED;
            Task.Delay(checkInterval.Add(TimeSpan.FromSeconds(1)));
        }

        [TestMethod]
        public void TestBrewingWontStartIfBoilerIsNotReady()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, null);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var watcher = new Watcher(boiler, warmer, brewController);

            watcher.TriggerBrewing();
            Assert.AreEqual(IndicatorState.INDICATOR_ON, _firmwareApiController.IndicatorState);
        }

        [TestMethod]
        public void TestBrewingWontStartIfWarmingPlateIsNotReady()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, null);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.IndicatorState = IndicatorState.INDICATOR_ON;
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;

            var watcher = new Watcher(boiler, warmer, brewController);

            watcher.TriggerBrewing();
            Assert.AreEqual(IndicatorState.INDICATOR_ON, _firmwareApiController.IndicatorState);
        }

        [TestMethod]
        public void TestBrewingWillStartIfWarmingPlateAndBoilerAreReady()
        {
            var warmer = new WarmerPlate(_firmwareApi);
            var boiler = new Boiler(_firmwareApi, warmer);
            var brewController = new BrewController(_firmwareApi, boiler, warmer);
            _firmwareApiController.BoilerStatus = BoilerStatus.BOILER_NOT_EMPTY;
            _firmwareApiController.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var watcher = new Watcher(boiler, warmer, brewController);

            watcher.TriggerBrewing();
            Assert.AreEqual(IndicatorState.INDICATOR_OFF, _firmwareApiController.IndicatorState);
        }

    }
}