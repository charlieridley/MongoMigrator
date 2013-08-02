using MongoDB.Driver;

namespace MongoMigrator
{
    public class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        public IMongoDatabase Create(string connectionString)
        {
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var client = new MongoClient(connectionString);
            var database = client.GetServer().GetDatabase(databaseName);
            return new MongoDatabase(database);
        }
    }
}