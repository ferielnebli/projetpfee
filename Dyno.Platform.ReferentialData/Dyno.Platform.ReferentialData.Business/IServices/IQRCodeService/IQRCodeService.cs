using Dyno.Platform.ReferentialData.DTO.PaymentDTO;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.IServices.IQRCodeService
{
    public interface IQRCodeService
    {
        Task<OperationResult<QRCodeInformationDTO>> GenerateQRCode(double amount, string userId);
    }
}
