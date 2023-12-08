using Dyno.Platform.ReferntialData.DataModel.RoleData;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Nhibernate.RoleData
{
    public class UserRoleMap: ClassMapping<UserRoleEntity>
    {
        public UserRoleMap()
        {
            /*Schema("public");
            Table("user_role");
            Id(e => e.UserId, id =>
            {
                id.Column("user_id");
                id.Type(NHibernateUtil.String);
                id.Length(64);
            });
            ManyToOne(x => x.User, m =>
            {
                m.Column("user_id"); // Foreign key column name
                m.ForeignKey("FK_User_Id"); // Foreign key constraint name
            });

            Id(e => e.Role, id =>
            {
                id.Column("role_id");
                id.Type(NHibernateUtil.String);
                id.Length(64);

            });
            ManyToOne(x => x.RoleId, m =>
            {
                m.Column("role_id"); // Foreign key column name
                m.ForeignKey("FK_Role_Id"); // Foreign key constraint name
            });*/
        }
    }
}
