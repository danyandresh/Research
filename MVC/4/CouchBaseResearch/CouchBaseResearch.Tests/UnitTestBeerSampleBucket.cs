using System.Threading.Tasks;
using Couchbase;
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

        [TestMethod]
        public void DifferentBucketsHaveTheSameCouchbaseClient()
        {
            // Due to expensive nature of instantiating a CouchbaseClient
            // it is recommendended to reuse the same instance across the application domain.
            var beerbucket = Kernel.Get<IBeerBucket>();

            var couchbaseBucket = (ICouchbaseBucket)beerbucket;
            var clientExpected = couchbaseBucket.CouchbaseClient;
            var computeClientForDifferentBucket = Task.Factory.StartNew(() =>
            {
                var differentKernel = SetupKernel();
                beerbucket = differentKernel.Get<IBeerBucket>();
                couchbaseBucket = (ICouchbaseBucket)beerbucket;
                return couchbaseBucket.CouchbaseClient;
            });

            var actualClient = computeClientForDifferentBucket.Result;

            Assert.AreEqual(clientExpected, actualClient);
        }
    }
}