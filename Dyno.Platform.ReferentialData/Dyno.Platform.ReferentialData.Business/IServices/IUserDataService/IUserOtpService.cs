using Platform.Shared.Enum;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Dyno.Platform.ReferentialData.Business.IServices.IUserDataService
{
    public interface IUserOtpService
    {
        OperationResult<string> GetOtpVerificationCode(string phoneNumber,  OtpType otpType);
        OperationResult<string> VerifierCode(string newCode, string phoneNumber, OtpType otpType);
    }
}
