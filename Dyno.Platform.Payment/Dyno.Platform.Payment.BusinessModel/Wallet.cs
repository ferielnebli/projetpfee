using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.BusinessModel
{
    public class Wallet
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PublicAddress { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public WalletType Type { get; set; }
        public double Balance { get; set; }
    }
}
