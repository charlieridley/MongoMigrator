namespace MongoMigrator
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase Create(string connectionstring);
    }
}