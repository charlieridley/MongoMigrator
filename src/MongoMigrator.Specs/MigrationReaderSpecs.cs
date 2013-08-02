using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;

namespace MongoMigrator.Specs
{
    [Subject(typeof(MigrationReader))]
    public class Reading_migrations_from_an_assembly : WithSubject<MigrationReader>
    {
        private static IEnumerable<IMigration> migrations;
        Because of = () => migrations = Subject.GetFromAssembly(typeof(FakeMigration1).Assembly);
        It should_have_the_first_migration = () => migrations.ShouldContain(x => x is FakeMigration1);
        It should_have_the_second_migration = () => migrations.ShouldContain(x => x is FakeMigration2);
    }
}
