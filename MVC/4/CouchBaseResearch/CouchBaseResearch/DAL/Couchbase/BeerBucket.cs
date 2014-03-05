using Couchbase;

namespace CouchBaseResearch.DAL.Couchbase
{
    public class BeerBucket : IBeerBucket, ICouchbaseBucket
    {
        private static CouchbaseClient _couchbaseClient;

        public ICouchbaseClient CouchbaseClient
        {
            get
            {
                return _couchbaseClient ?? (_couchbaseClient = new CouchbaseClient());
            }
        }
    }
}