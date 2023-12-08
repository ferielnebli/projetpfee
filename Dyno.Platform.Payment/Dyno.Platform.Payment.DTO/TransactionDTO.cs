using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.DTO
{
    public class TransactionDTO
    {
        public string Id { get; set; }  = Guid.NewGuid().ToString();
        public string SendUserId { get; set; } = string.Empty;
        public string ReceiveUserId { get; set; } = string.Empty;
        public string ReceiveName { get; set; } = string.Empty;
        public DateTime  Date { get; set; } = DateTime.Now;
        public QRCodeDTO QRCode { get; set; } = new QRCodeDTO();
    }
}
