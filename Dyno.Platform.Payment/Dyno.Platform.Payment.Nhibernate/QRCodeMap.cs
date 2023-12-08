using Dyno.Platform.Payment.DataModel;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Dyno.Platform.Payment.Nhibernate
{
    public class QRCodeMap : ClassMapping<QRCodeEntity>
    {
        public QRCodeMap()
        {
            Table("qrcode");
            Id(qrcode => qrcode.Id, x =>
            {
                x.Column("id");
                x.Type(NHibernateUtil.StringClob);
            });

            Property(qrcode => qrcode.Code, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("code");
            });

            Property(qrcode => qrcode.Amount, x =>
            {
                x.Type(NHibernateUtil.Double);
                x.Column("amount");
            });

            Property(qrcode => qrcode.CreationDate, x =>
            {

                x.Type(NHibernateUtil.UtcDateTime);
                x.Column("creation_date");

            });

            Property(qrcode => qrcode.ExpiredDate, x =>
            {

                x.Type(NHibernateUtil.UtcDateTime);
                x.Column("expired_date");

            });

            Property(qrcode => qrcode.Status, x =>
            {

                x.Type(NHibernateUtil.Int32);
                x.Column("status");

            });


        }
    }
}
