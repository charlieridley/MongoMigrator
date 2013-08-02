namespace MongoMigrator
{
    public interface IMigration
    {
        void Up(IMongoDatabase mongoDatabase);
        int GetVersion();
    }
}