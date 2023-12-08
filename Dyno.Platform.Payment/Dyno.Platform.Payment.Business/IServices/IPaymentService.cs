using Dyno.Platform.Payment.DTO;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Business.IServices
{
    public interface IPaymentService 
    {
        OperationResult<string> Payer(PayerDTO payer);
    }
}
