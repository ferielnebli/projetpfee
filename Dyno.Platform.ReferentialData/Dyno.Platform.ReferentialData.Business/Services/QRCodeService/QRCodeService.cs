using Dyno.Platform.Payment.DTO;
using Dyno.Platform.ReferentialData.Business.Helper;
using Dyno.Platform.ReferentialData.Business.IServices.IQRCodeService;
using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.DTO.PaymentDTO;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Services.QRCodeService
{
    public class QRCodeService : IQRCodeService
    {
        public readonly IUserService _userService;
        private readonly IPaymentHelper _paymentHelper;
        public QRCodeService(IUserService userService,
            IPaymentHelper paymentHelper) 
        { 
            _userService = userService;
            _paymentHelper = paymentHelper;
        }


        public async Task<OperationResult<QRCodeInformationDTO>> GenerateQRCode(double amount, string userId)
        {
            OperationResult<UserDTO> user = await _userService.GetById(userId);
            if(user == null || user.ObjectValue == null)
            {
                return new OperationResult<QRCodeInformationDTO>
                {
                    Result = QueryResult.UnAuthorized,
                    ExceptionMessage = "User unauthorized !"
                };
            }
            //Create QRCodes
            OperationResult<QRCodeDTO>? createQRcode = _paymentHelper.createQRCode(userId, amount);

            if(createQRcode == null || createQRcode.Result != QueryResult.IsSucced)
            {
                return new OperationResult<QRCodeInformationDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "Problem in generate qrcode !"
                };
            }

            return new OperationResult<QRCodeInformationDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = new QRCodeInformationDTO
                {
                    Amount = amount,
                    Code = createQRcode.ObjectValue != null ? createQRcode.ObjectValue.Code : 0,
                    UserId = userId,
                    UserName = user.ObjectValue.FirstName,
                    CreationDate = createQRcode.ObjectValue?.CreationDate,
                    ExpiredDate = createQRcode.ObjectValue?.ExpiredDate
                }
            };
        }

        
    }
}
