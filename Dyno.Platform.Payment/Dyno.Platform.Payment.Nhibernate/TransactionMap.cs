using Dyno.Platform.Payment.DataModel;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Nhibernate
{
    public class TransactionMap : ClassMapping<TransactionEntity>
    {
        public TransactionMap()
        {
            Table("transcation");
            Id(transaction => transaction.Id, x =>
            {
                x.Column("id");
                x.Type(NHibernateUtil.StringClob);
            });

            Property(transaction => transaction.ReceiveUserId, x =>
            {
                x.Type(NHibernateUtil.StringClob);
                x.Column("receiver_user_id");
            });

            Property(transaction => transaction.ReceiveName, x =>
            {
                x.Type(NHibernateUtil.StringClob);
                x.Column("receiver_user_name");
            });

            Property(transaction => transaction.SendUserId, x =>
            {

                x.Type(NHibernateUtil.StringClob);
                x.Column("sender_user_id");

            });

            Property(transaction => transaction.Date, x =>
            {

                x.Type(NHibernateUtil.DateTime);
                x.Column("date");

            });

            ManyToOne(transaction => transaction.QRCode, x =>
            {
                x.Column("qrcode_id"); 
                x.Cascade(Cascade.All);
            });

        }
    }
}
