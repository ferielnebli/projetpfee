using Dyno.Platform.ReferntialData.DataModel.UserClaim;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Nhibernate.UserClaimData
{
    public class UserLoginMap : ClassMapping<UserLoginEntity>
    {
        public UserLoginMap()
        {
            Schema("public");
            Table("user_login");
            Property(e => e.UserId, prop =>
            {
                prop.Column("user_id");
                prop.Type(NHibernateUtil.String);
                prop.Length(32);
                prop.NotNullable(true);
                prop.Unique(true);
            });


            Property(e => e.LoginProvider, prop =>
            {
                prop.Column("login_provider");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);

            });

            Property(e => e.ProviderDisplayName, prop =>
            {
                prop.Column("provider_display_name");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);

            });


            Property(e => e.ProviderKey, prop =>
            {
                prop.Column("provider_key");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);

            });
        }
    }
}
