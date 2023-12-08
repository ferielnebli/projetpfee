using Dyno.Platform.ReferntialData.DataModel.UserRole;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Nhibernate.RoleData
{
    public class RoleMap : ClassMapping<RoleEntity>
    {
        public RoleMap()
        {


            Schema("public");
            Table("Roles");
            Id(e => e.Id, id =>
            {
                id.Column("id");
                id.Type(NHibernateUtil.String);
                id.Length(64);

            });
            Property(e => e.Name, prop =>
            {
                prop.Column("name");
                prop.Type(NHibernateUtil.String);
                prop.Length(64);
                prop.NotNullable(true);
                prop.Unique(true);
            });
            Property(e => e.NormalizedName, prop =>
            {
                prop.Column("normalized_name");
                prop.Type(NHibernateUtil.String);
                prop.Length(64);
                prop.NotNullable(true);
                prop.Unique(true);
            });
            Property(e => e.ConcurrencyStamp, prop =>
            {
                prop.Column("concurrency_stamp");
                prop.Type(NHibernateUtil.String);
                prop.Length(36);                                             
                prop.NotNullable(false);
            });




            Bag(role => role.Users, X =>
            {

                X.Table("user_role");

                X.Key(k => k.Column("role_id"));

                X.Lazy(CollectionLazy.Lazy);
                X.Cascade(Cascade.All);
            }, r => r.ManyToMany(m => m.Column("user_id")));


        }
    }
}
