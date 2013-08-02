using System;

namespace MongoMigrator.Specs
{
    [Migration(1)]
    public class FakeMigration1 : Migration
    {
        public override void Up(IMongoDatabase mongoDatabase)
        {
            throw new NotImplementedException();
        }
    }

    [Migration(2)]
    public class FakeMigration2 : Migration
    {
        public override void Up(IMongoDatabase mongoDatabase)
        {
            throw new NotImplementedException();
        }
    }
}
