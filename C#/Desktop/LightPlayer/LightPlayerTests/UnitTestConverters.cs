using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer.Converters;
using System.Windows.Controls;
using LightPlayer;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestConverters : TestContext
    {
        [TestMethod]
        public void TestMethodMultiValueParamsConverterMediaElementConvertsToMediaElementProxy()
        {
            var converter = WindsorContainer.Resolve<MultiValueParams>();

            var mediaElement = new MediaElement();
            var file = string.Empty;

            var tuple = converter.Convert(new dynamic[] { mediaElement, file }, null, null, null);

            Assert.IsTrue(tuple is Tuple<IMediaElement, string>);
        }
    }
}
