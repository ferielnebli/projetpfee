using Dyno.Platform.ReferntialData.DataModel.UserRole;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.AspNetCore.Identity;

namespace Dyno.Platform.ReferentialData.Nhibernate.RoleData
{
    public class RoleClaimMap:ClassMapping<IdentityRoleClaim>
    {
        public RoleClaimMap()
        {
            Schema("public");
            Table("role_claim");
            Id(e => e.Id, id => {
                id.Column("id");
                id.Type(NHibernateUtil.Int32);
                id.Generator(Generators.Identity);
               
                
            });
            Property(e => e.RoleId, prop => {
                prop.Column("role_id");
                prop.Type(NHibernateUtil.String);
                prop.Length(32);
                prop.NotNullable(true);
                
            });
            Property(e => e.ClaimType, prop => {
                prop.Column("claim_type");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);
                
            });
            Property(e => e.ClaimValue, prop => {
                prop.Column("claim_value");
                prop.Type(NHibernateUtil.String);
                prop.Length(256);
                prop.NotNullable(true);
            });
        }
    }
}
