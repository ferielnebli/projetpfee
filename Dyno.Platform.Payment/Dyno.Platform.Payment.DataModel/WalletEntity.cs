using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.DataModel
{
    public class WalletEntity
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual string PublicAddress { get; set; } = string.Empty;
        public virtual string Address { get; set; } = string.Empty;
        public virtual string PrivateKey { get; set; } = string.Empty;
        public virtual string UserId { get; set; } = string.Empty;
        public virtual WalletType Type { get; set; }
        public virtual double Balance { get; set; }
    }
}
