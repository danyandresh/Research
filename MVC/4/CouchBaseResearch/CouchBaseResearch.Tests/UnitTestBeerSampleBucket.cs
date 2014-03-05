using CouchBaseResearch.DAL;
using CouchBaseResearch.DAL.Couchbase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace CouchBaseResearch.Tests
{
    [TestClass]
    public class UnitTestBeerSampleBucket
    {
        [TestMethod]
        public void BucketHasCouchbaseClient()
        {
            using (var kernel = new StandardKernel())
            {
                //TODO: Use convention over configuration
                kernel.Bind<IBeerBucket>().To<BeerBucket>();

                var beerbucket = kernel.Get<IBeerBucket>();

                var couchbaseBucket = beerbucket as ICouchbaseBucket;
                Assert.IsNotNull(couchbaseBucket);
                Assert.IsNotNull(couchbaseBucket.CouchbaseClient);
            }
        }
    }
}