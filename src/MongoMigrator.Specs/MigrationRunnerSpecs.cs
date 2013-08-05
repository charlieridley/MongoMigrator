using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace MongoMigrator.Specs
{
    [Subject(typeof(MigrationRunner))]
    public class When_running_migrations_when_there_is_a_new_one : WithSubject<MigrationRunner>
    {
        private static Mock<IModelCollection<VersionInfo>> modelCollection;
        private static IQueryable<VersionInfo> versions;
        private static IEnumerable<IMigration> migrations;
        private static Mock<IMongoDatabase> mongoDatabase;
        private static Mock<IMigration> migration1;
        private static Mock<IMigration> migration2;
        private static Mock<IMigration> migration3;
        private static DateTime dateTime;
        Establish context = () =>
            {
                dateTime = DateTime.Now;
                The<IDateTimeWrapper>().WhenToldTo(x => x.Now()).Return(dateTime);
                migration1 = new Mock<IMigration>();
                migration2 = new Mock<IMigration>();
                migration3 = new Mock<IMigration>();
                migration1.Setup(x => x.GetVersion()).Returns(1);
                migration2.Setup(x => x.GetVersion()).Returns(2);
                migration3.Setup(x => x.GetVersion()).Returns(3);
                migrations = new[] { migration1.Object, migration2.Object, migration3.Object };
                versions = new[] {new VersionInfo {Version = 1}, new VersionInfo {Version = 2}}.AsQueryable();
                modelCollection = new Mock<IModelCollection<VersionInfo>>();
                modelCollection.Setup(x => x.Get()).Returns(versions);
                mongoDatabase = new Mock<IMongoDatabase>();
                The<IMongoDatabaseFactory>().WhenToldTo(x => x.Create(Param<string>.IsAnything)).Return(mongoDatabase.Object);
                mongoDatabase.Object.WhenToldTo(x => x.GetCollection<VersionInfo>()).Return(modelCollection.Object);
                The<IMigrationReader>().WhenToldTo(x => x.GetFromAssembly(Param<Assembly>.IsAnything)).Return(migrations);
            };
        Because of = () => Subject.Run(new MigrationOptions{ConnectionString = "connectionstring", Assembly = typeof(FakeMigration1).Assembly});
        It should_get_the_mongo_database = () => The<IMongoDatabaseFactory>().WasToldTo(x => x.Create("connectionstring"));
        It should_get_the_migrations_that_have_been_run_from_the_database = () => modelCollection.Object.WasToldTo(x => x.Get());
        It should_get_a_list_of_migrations_from_the_assembly = () => The<IMigrationReader>().WasToldTo(x => x.GetFromAssembly(typeof(FakeMigration1).Assembly));
        It should_not_run_the_first_migration = () => migration1.Object.WasNotToldTo(x => x.Up(mongoDatabase.Object));
        It should_not_run_the_second_migration = () => migration2.Object.WasNotToldTo(x => x.Up(mongoDatabase.Object));
        It should_run_the_third_migration = () => migration3.Object.WasToldTo(x => x.Up(mongoDatabase.Object));
        It should_insert_a_document_in_the_version_info_collection = () => modelCollection.Verify(x => x.Save(Moq.It.Is((VersionInfo m) => m.Version == 3 && m.AppliedOn == dateTime)));
        
    }

    [Subject(typeof (MigrationRunner))]
    public class When_running_migrations_when_there_are_no_new_ones : WithSubject<MigrationRunner>
    {
        private static Mock<IModelCollection<VersionInfo>> modelCollection;
        private static IQueryable<VersionInfo> versions;
        private static IEnumerable<IMigration> migrations;
        private static Mock<IMongoDatabase> mongoDatabase;
        private static Mock<IMigration> migration1;
        private static Mock<IMigration> migration2;
        Establish context = () =>
        {
            migration1 = new Mock<IMigration>();
            migration2 = new Mock<IMigration>();
            migration1.Setup(x => x.GetVersion()).Returns(1);
            migration2.Setup(x => x.GetVersion()).Returns(2);
            migrations = new[] { migration1.Object, migration2.Object };
            versions = new[] { new VersionInfo { Version = 1 }, new VersionInfo { Version = 2 } }.AsQueryable();
            modelCollection = new Mock<IModelCollection<VersionInfo>>();
            modelCollection.Setup(x => x.Get()).Returns(versions);
            mongoDatabase = new Mock<IMongoDatabase>();
            The<IMongoDatabaseFactory>().WhenToldTo(x => x.Create(Param<string>.IsAnything)).Return(mongoDatabase.Object);
            mongoDatabase.Object.WhenToldTo(x => x.GetCollection<VersionInfo>()).Return(modelCollection.Object);
            The<IMigrationReader>().WhenToldTo(x => x.GetFromAssembly(Param<Assembly>.IsAnything)).Return(migrations);
        };
        Because of = () => Subject.Run(new MigrationOptions { ConnectionString = "connectionstring", Assembly = Assembly.GetCallingAssembly() });
        It should_not_run_the_first_migration = () => migration1.Object.WasNotToldTo(x => x.Up(mongoDatabase.Object));
        It should_not_run_the_second_migration = () => migration2.Object.WasNotToldTo(x => x.Up(mongoDatabase.Object));
    }

    [Subject(typeof (MigrationRunner))]
    public class When_running_migrations_which_are_not_in_order : WithSubject<MigrationRunner>
    {
        private static Mock<IModelCollection<VersionInfo>> modelCollection;
        private static IQueryable<VersionInfo> versions;
        private static IEnumerable<IMigration> migrations;
        private static Mock<IMongoDatabase> mongoDatabase;
        private static Mock<IMigration> migration1;
        private static Mock<IMigration> migration2;
        private static Mock<IMigration> migration3;
        private static List<int> migrationOrder;
        Establish context = () =>
            {
                migrationOrder = new List<int>();
                migration1 = new Mock<IMigration>();
                migration2 = new Mock<IMigration>();
                migration3 = new Mock<IMigration>();
                migration1.Setup(x => x.GetVersion()).Returns(1);
                migration2.Setup(x => x.GetVersion()).Returns(2);
                migration3.Setup(x => x.GetVersion()).Returns(3);
                migration1.Setup(x => x.Up(Moq.It.IsAny<IMongoDatabase>())).Callback(() => migrationOrder.Add(1));
                migration2.Setup(x => x.Up(Moq.It.IsAny<IMongoDatabase>())).Callback(() => migrationOrder.Add(2));
                migration3.Setup(x => x.Up(Moq.It.IsAny<IMongoDatabase>())).Callback(() => migrationOrder.Add(3));
                migrations = new[] { migration3.Object, migration1.Object, migration2.Object };
                versions = new[] { new VersionInfo { Version = 1 } }.AsQueryable();
                modelCollection = new Mock<IModelCollection<VersionInfo>>();
                modelCollection.Setup(x => x.Get()).Returns(versions);
                mongoDatabase = new Mock<IMongoDatabase>();
                The<IMongoDatabaseFactory>().WhenToldTo(x => x.Create(Param<string>.IsAnything)).Return(mongoDatabase.Object);
                mongoDatabase.Object.WhenToldTo(x => x.GetCollection<VersionInfo>()).Return(modelCollection.Object);
                The<IMigrationReader>().WhenToldTo(x => x.GetFromAssembly(Param<Assembly>.IsAnything)).Return(migrations);
            };
        Because of = () => Subject.Run(new MigrationOptions { ConnectionString = "connectionstring", Assembly = Assembly.GetCallingAssembly() });
        It should_run_the_migrations = () => migrationOrder.ShouldBeLike(new[]{2,3});
    }
}
