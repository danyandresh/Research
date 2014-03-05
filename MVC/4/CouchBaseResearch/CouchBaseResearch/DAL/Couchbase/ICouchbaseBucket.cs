using Couchbase;

namespace CouchBaseResearch.DAL.Couchbase
{
    public interface ICouchbaseBucket
    {
        ICouchbaseClient CouchbaseClient { get; }
    }
}