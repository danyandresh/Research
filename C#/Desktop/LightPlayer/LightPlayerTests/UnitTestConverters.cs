using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightPlayer.Converters;

namespace LightPlayerTests
{
    [TestClass]
    public class UnitTestConverters : TestContext
    {
        [TestMethod]
        public void TestMethodMultiValueParamsConverterReturnsTupleOfTwoParams()
        {
            var converter=WindsorContainer.Resolve<MultiValueParams>();
            var tuple = converter.Convert(new dynamic[] { null, null }, null, null, null);

            Assert.IsTrue(tuple is Tuple<dynamic, dynamic>);
        }
    }
}
