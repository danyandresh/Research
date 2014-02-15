using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using LightPlayer;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestMediaElementProxy : TestContext
    {
        [TestMethod]
        public void TestMethodMediaElementProxySurrogatesPlay()
        {
            var mediaElement = new MediaElement();
            IMediaElement proxy = MediaElementProxy.BuildProxy(mediaElement);
            var playMethod=typeof(MediaElement).GetMethod("Play");

            Assert.AreEqual(playMethod,((Delegate)proxy.Play).Method, "Play method is not surrogated");
            Assert.AreEqual(mediaElement,((System.Delegate)proxy.Play).Target,"MediaElement surrogate's play method does not reference original media element");
        }

        [TestMethod]
        public void TestMethodMediaElementProxySurrogatesStop()
        {
            var mediaElement = new MediaElement();
            IMediaElement proxy = MediaElementProxy.BuildProxy(mediaElement);
            var stopMethod = typeof(MediaElement).GetMethod("Stop");

            Assert.AreEqual(stopMethod, ((Delegate)proxy.Stop).Method, "Stop method is not surrogated");
            Assert.AreEqual(mediaElement, ((System.Delegate)proxy.Play).Target, "MediaElement surrogate's stop method does not reference original media element");
        }
    }
}
