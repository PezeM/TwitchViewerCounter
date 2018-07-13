using MongoDB.Driver;
using TwitchViewerCounter.Database.Entities;

namespace TwitchViewerCounter.Database.Repositories
{
    public class StreamerRepository : BaseMongoRepository<Streamer>
    {
        private const string StreamerCollectionName = "Streamers";
        private readonly MongoDataContext _dataContext;

        public StreamerRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        protected override IMongoCollection<Streamer> Collection
        {
            get
            {
                return _dataContext.MongoDatabase.GetCollection<Streamer>(StreamerCollectionName);
            }
        }
    }
}
