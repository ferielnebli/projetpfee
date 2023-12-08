using Dyno.Platform.ReferntialData.DataModel.UserClaim;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using NHibernate.AspNetCore.Identity;
using NHibernate.Mapping;

namespace Dyno.Platform.ReferentialData.Nhibernate.UserClaimData
{
    public class UserClaimMap : ClassMapping<IdentityUserClaim>
    {

        public UserClaimMap()
        {
            Schema("public");
            Table("user_claim");
            Id(e => e.Id, id =>
            {
                id.Column("id");
                id.Type(NHibernateUtil.Int32);
                

            });
           

            Property(e => e.UserId, prop =>
            {
                prop.Column("user_id");
                prop.Type(NHibernateUtil.String);
                prop.NotNullable(true);

            });


            Property(e => e.ClaimType, prop =>
            {
                prop.Column("claim_type");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);

            });
            Property(e => e.ClaimValue, prop =>
            {
                prop.Column("claim_value");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);
            });
        }
    }
}
