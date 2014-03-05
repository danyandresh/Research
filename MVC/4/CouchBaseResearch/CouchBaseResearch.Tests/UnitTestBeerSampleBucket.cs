using CouchBaseResearch.DAL;
using CouchBaseResearch.DAL.Couchbase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace CouchBaseResearch.Tests
{
    [TestClass]
    public class UnitTestBeerSampleBucket : TestContext
    {
        [TestMethod]
        public void BucketHasCouchbaseClient()
        {
            var beerbucket = Kernel.Get<IBeerBucket>();

            var couchbaseBucket = beerbucket as ICouchbaseBucket;
            Assert.IsNotNull(couchbaseBucket);
            Assert.IsNotNull(couchbaseBucket.CouchbaseClient);
        }
    }
}