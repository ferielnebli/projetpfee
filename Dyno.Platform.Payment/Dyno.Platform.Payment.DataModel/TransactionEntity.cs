using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.DataModel
{
    public class TransactionEntity
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual string SendUserId { get; set; } = string.Empty;
        public virtual string ReceiveUserId { get; set; } = string.Empty;
        public virtual string ReceiveName { get; set; } = string.Empty;

        public virtual  DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("qrcode_id")]
        public virtual QRCodeEntity QRCode { get; set; } = new QRCodeEntity();       
    }
}
