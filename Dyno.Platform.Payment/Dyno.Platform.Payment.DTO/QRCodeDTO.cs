using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.DTO
{
    public class QRCodeDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Code { get; set; }
        public double Amount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public QRStatus Status { get; set; }
        public string UserCreatorId { get; set; } = string.Empty;
        public string UserScanerId { get; set; } = string.Empty;
    }
}
