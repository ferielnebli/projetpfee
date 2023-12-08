using Dyno.Platform.Payment.DTO;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Helper
{
    public interface IPaymentHelper
    {
        OperationResult<WalletDTO>? createWallet(string userId, WalletType type);
        OperationResult<QRCodeDTO>? createQRCode(string userId, double amount);
        OperationResult<WalletDTO>? GetBalance(string userId);
        OperationResult<List<TransactionDTO>>? GetTransactions(string userId);
    }
}
