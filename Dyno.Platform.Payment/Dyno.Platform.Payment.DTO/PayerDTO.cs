using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.DTO
{
    public class PayerDTO
    {
        public int Code { get; set; }

        public double Amount { get; set; }

        public string scannerUserId { get; set; } = string.Empty;
    }
}
