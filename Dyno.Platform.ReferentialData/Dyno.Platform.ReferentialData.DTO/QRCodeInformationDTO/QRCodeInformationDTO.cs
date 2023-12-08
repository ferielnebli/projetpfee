using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.PaymentDTO
{
    public class QRCodeInformationDTO
    {
        public int Code { get; set; }

        public string UserId { get; set; } = Guid.NewGuid().ToString();

        public string UserName { get; set; } = string.Empty;

        public double Amount { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ExpiredDate { get; set; }
    }
}
