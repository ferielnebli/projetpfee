using Dyno.Platform.Payment.BusinessModel;
using Dyno.Platform.Payment.DTO;
using Platform.Shared.Enum;
using Platform.Shared.HttpHelper;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Helper
{
    public class PaymentHelper : IPaymentHelper
    {
        private readonly IHelper<TransactionDTO> _transactionHelper;
        private readonly IHelper<QRCodeDTO> _qrcodeHelper;
        private readonly IHelper<WalletDTO> _walletHelper;

        public PaymentHelper(IHelper<TransactionDTO> transactionHelper,
            IHelper<QRCodeDTO> qrcodeHelper,
            IHelper<WalletDTO> walletHelper)
        {
            _transactionHelper = transactionHelper;
            _qrcodeHelper = qrcodeHelper;
            _walletHelper = walletHelper;
        }

        public OperationResult<QRCodeDTO>? createQRCode(string userId, double amount)
        {
            QRCodeDTO qRCodeDTO = new QRCodeDTO
            {
                Amount = amount,
                UserCreatorId = userId
            };

            string url = $"https://192.168.1.9:5259/api/QRCode/Create";
            OperationResult<QRCodeDTO>? result = _qrcodeHelper.Post(url, qRCodeDTO);

            return result;
        }

        public OperationResult<WalletDTO>? createWallet(string userId, WalletType type)
        {
            WalletDTO wallet = new WalletDTO
            {
                UserId = userId,
                Type = type 
            };
            string url = $"https://192.168.1.9:5259/api/Wallet/Create";
            OperationResult<WalletDTO>? result = _walletHelper.Post(url, wallet);

            return result;
        }

        public OperationResult<WalletDTO>? GetBalance(string userId)
        {
            string url = $"https://192.168.1.9:5259/api/Wallet/GetByUserId";
            OperationResult<WalletDTO>? result = _walletHelper.Get(url, userId);

            return result;
        }

        public OperationResult<List<TransactionDTO>>? GetTransactions(string userId)
        {
            string url = $"https://192.168.1.9:5259/api/Transaction/GetByUserId?userId={userId}";
            OperationResult<List<TransactionDTO>>? result = _transactionHelper.GetAll(url);

            return result;
        }


    }
}
