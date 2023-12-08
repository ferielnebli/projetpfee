using Dyno.Platform.Payment.DataModel;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Nhibernate
{
    public class WalletMap : ClassMapping<WalletEntity>
    {
        public WalletMap()
        {
            Table("wallet");
            Id(wallet => wallet.Id, x =>
            {
                x.Column("id");
                x.Type(NHibernateUtil.StringClob);
            });

            Property(wallet => wallet.PublicAddress, x =>
            {
                x.Type(NHibernateUtil.StringClob);
                x.Column("public_address");
            });

            Property(wallet => wallet.Address, x =>
            {
                x.Type(NHibernateUtil.StringClob);
                x.Column("address");
            });

            Property(wallet => wallet.PrivateKey, x =>
            {

                x.Type(NHibernateUtil.StringClob);
                x.Column("private_key");

            });
            Property(wallet => wallet.Balance, x =>
            {

                x.Type(NHibernateUtil.Double);
                x.Column("balance");

            });

            Property(wallet => wallet.Type, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("type");
            });

            Property(wallet => wallet.UserId, x =>
            {

                x.Type(NHibernateUtil.StringClob);
                x.Column("user_id");

            });

        }
    }
}
