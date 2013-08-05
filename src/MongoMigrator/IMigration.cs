namespace MongoMigrator
{
    public interface IMigration
    {
        void Up(IMongoDatabase mongoDatabase);
        long GetVersion();
    }
}