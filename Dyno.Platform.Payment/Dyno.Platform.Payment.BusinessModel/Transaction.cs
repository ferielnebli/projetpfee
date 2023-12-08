using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.BusinessModel
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SendUserId { get; set; } = string.Empty;
        public string ReceiveUserId { get; set; } = string.Empty;
        public string ReceiveName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public QRCode QRCode { get; set; } = new QRCode();
    }
}
