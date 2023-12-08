using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Dyno.Platform.Payment.DataModel
{
    public class QRCodeEntity
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual int Code { get; set; }
        public virtual double Amount { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ExpiredDate { get; set; }
        public virtual QRStatus Status { get; set; }
        public virtual string UserCreatorId { get; set; } = string.Empty;
        public virtual string UserScanerId { get; set; } = string.Empty;

        [ForeignKey("transaction_id")]
        public virtual TransactionEntity? Transaction { get; set; }
        public virtual string? TransactionId { get; set; }
    }
}
