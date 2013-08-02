using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace MongoMigrator.IntegrationSpecs
{
    [Subject(typeof(MigrationRunner))]
    public class Running_migrations_with_empty_database
    {
        private static MigrationRunner migrationRunner;
        Establish context = () =>
            {
                MigrationState.Migration1Executed = false;
                MigrationState.Migration2Executed = false;
                var database = new MongoDatabaseFactory().Create("mongodb://localhost:27017/mongoMigrator");
                database.GetCollection<VersionInfo>().RemoveAll();
                migrationRunner = new MigrationRunner();
            };
        Because of = () => migrationRunner.Run(new MigrationOptions { Assembly = typeof(FakeMigration1).Assembly, ConnectionString = "mongodb://localhost:27017/mongoMigrator" });
        It should_execute_migration1 = () => MigrationState.Migration1Executed.ShouldBeTrue();
        It should_execute_migration2 = () => MigrationState.Migration2Executed.ShouldBeTrue();
    }

    [Subject(typeof(MigrationRunner))]
    public class Running_migrations_when_an_existing_migration_has_been_run
    {
        private static MigrationRunner migrationRunner;
        Establish context = () =>
        {
            MigrationState.Migration1Executed = false;
            MigrationState.Migration2Executed = false;
            var database = new MongoDatabaseFactory().Create("mongodb://localhost:27017/mongoMigrator");
            var versionInfoCollection = database.GetCollection<VersionInfo>();
            versionInfoCollection.RemoveAll();
            versionInfoCollection.Save(new VersionInfo{Version = 1});
            migrationRunner = new MigrationRunner();
        };
        Because of = () => migrationRunner.Run(new MigrationOptions { Assembly = typeof(FakeMigration1).Assembly, ConnectionString = "mongodb://localhost:27017/mongoMigrator" });
        It should_not_execute_migration1 = () => MigrationState.Migration1Executed.ShouldBeFalse();
        It should_execute_migration2 = () => MigrationState.Migration2Executed.ShouldBeTrue();
    }
}
