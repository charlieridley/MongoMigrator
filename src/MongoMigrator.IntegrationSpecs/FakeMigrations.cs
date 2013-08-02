namespace MongoMigrator.IntegrationSpecs
{
    public static class MigrationState
    {
        static MigrationState()
        {
            Migration1Executed = false;
            Migration2Executed = false;
        }
        public static bool Migration1Executed { get; set; }
        public static bool Migration2Executed { get; set; }
    }
    
    [Migration(1)]
    public class FakeMigration1 : Migration
    {
        public override void Up(IMongoDatabase mongoDatabase)
        {
            MigrationState.Migration1Executed = true;
        }
    }

    [Migration(2)]
    public class FakeMigration2 : Migration
    {
        public override void Up(IMongoDatabase mongoDatabase)
        {
            MigrationState.Migration2Executed = true;
        }
    }
}
