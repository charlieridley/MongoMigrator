using System;
using System.Linq;

namespace MongoMigrator
{
    public class MigrationRunner
    {
        private readonly IMongoDatabaseFactory mongoDatabaseFactory;
        private readonly IMigrationReader migrationReader;

        public MigrationRunner() : this(new MongoDatabaseFactory(), new MigrationReader())
        {
            
        }
        
        public MigrationRunner(IMongoDatabaseFactory mongoDatabaseFactory, IMigrationReader migrationReader)
        {
            this.mongoDatabaseFactory = mongoDatabaseFactory;
            this.migrationReader = migrationReader;
        }

        public void Run(MigrationOptions migrationOptions)
        {
            if (migrationOptions == null) throw new ArgumentNullException("migrationOptions");
            var database = mongoDatabaseFactory.Create(migrationOptions.ConnectionString);
            var versionCollection = database.GetCollection<VersionInfo>();
            var existingVersions = versionCollection.Get().ToArray();
            var migrations = migrationReader.GetFromAssembly(migrationOptions.Assembly);
            var newMigrations = migrations.Where(x => existingVersions.All(m => m.Version != x.GetVersion()));
            foreach (var newMigration in newMigrations.OrderBy(x => x.GetVersion()))
            {
                newMigration.Up(database);
                versionCollection.Save(new VersionInfo{Version = newMigration.GetVersion()});
            }
        }
    }
}
