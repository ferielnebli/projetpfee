using Dyno.Platform.ReferntialData.DataModel.UserData;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Nhibernate.UserData
{
    public class UserTokenMap : ClassMapping<UserTokenEntity>
    {
        public UserTokenMap()
        {
            Table("user_token");
            Id(userToken => userToken.Id, x =>
            {
                x.Column("id");
                x.Type(NHibernateUtil.StringClob);
            });

            Property(userToken => userToken.Token, x =>
            {
                x.Type(NHibernateUtil.StringClob);
                x.Column("token");
            });

            Property(userToken => userToken.RefreshToken, x =>
            {

                x.Type(NHibernateUtil.StringClob);
                x.Column("refresh_token");

            });

            Property(userToken => userToken.ExpiredDate, x =>
            {

                x.Type(NHibernateUtil.DateTime);
                x.Column("date");

            });

            ManyToOne(userToken => userToken.User, x =>
            {
                x.Column("user_id");
                x.Cascade(Cascade.All);
            });

        }
    }
}
