using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace MongoMigrator.Specs
{
    public class When_getting_a_version_number
    {
        It should_get_the_version_number_from_the_attribute = () => new FakeMigration2().GetVersion().ShouldEqual(2);
    }
}
