using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dyno.Platform.ReferentialData.Nhibernate.Migration
{
    using FluentMigrator;

    [Migration(1)]
    public class AddPersonTable : Migration
    {
        public override void Up()
        {
            Create.Table("Person")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(50)
                .WithColumn("LastName").AsString(50);
        }

        public override void Down()
        {
            Delete.Table("Person");
        }
    }
}
