using MongoDB.Driver;
using System.Configuration;

namespace TwitchViewerCounter.Database
{
    public class MongoDataContext
    {
        public IMongoDatabase MongoDatabase { get; }

        public MongoDataContext() : this("MongoDb")
        {
        }

        public MongoDataContext(string connectionName)
        {
            var url = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            var mongoUrl = new MongoUrl(url);
            IMongoClient client = new MongoClient(mongoUrl);
            MongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);
        }

    }
}
