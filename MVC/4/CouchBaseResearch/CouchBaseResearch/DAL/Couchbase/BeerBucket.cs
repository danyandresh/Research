using Couchbase;

namespace CouchBaseResearch.DAL.Couchbase
{
    public class BeerBucket : IBeerBucket, ICouchbaseBucket
    {
        public ICouchbaseClient CouchbaseClient
        {
            get
            {
                return new CouchbaseClient();
            }
        }
    }
}