using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.DTO;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IQRCodeService _qrcodeService;
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        public PaymentService(IQRCodeService qrcodeService,
            ITransactionService transactionService,
            IWalletService walletService) 
        {
            _qrcodeService = qrcodeService;
            _transactionService = transactionService;
            _walletService = walletService;
        }

        public OperationResult<string> Payer(PayerDTO payer)
        {
            //check QRCode Exist  then modifier to validate
            OperationResult<List<QRCodeDTO>> result = _qrcodeService.Get(qrcode => qrcode.Code == payer.Code &&
            qrcode.ExpiredDate > DateTime.Now);

            if(result.Result != QueryResult.IsSucced || result.ObjectValue?.Count == 0)
            {
                return new OperationResult<string>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "QRCode Invalid"
                };
            }

            //check if wallet of scanner has amount or not in his balance

            OperationResult<List<WalletDTO>> wallet = _walletService.Get(wallet => wallet.UserId == payer.scannerUserId);
            if(wallet == null || 
                wallet.Result != QueryResult.IsSucced || 
                wallet.ObjectValue == null ||
                wallet.ObjectValue.Count == 0) 
            {
                return new OperationResult<string>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "wallet unfound"
                };
            }

            if(wallet.ObjectValue.FirstOrDefault()?.Balance < payer.Amount)
            {
                return new OperationResult<string>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "Balance unsuffissante !"
                };
            }

            if(result.ObjectValue?.FirstOrDefault() != null ) 
            {
                if(wallet.ObjectValue.FirstOrDefault() != null)
                {

                    WalletDTO walletDto = wallet.ObjectValue.FirstOrDefault()?? new WalletDTO();
                    QRCodeDTO qRCodeDTO = result.ObjectValue.FirstOrDefault()?? new QRCodeDTO();


                    //create Transaction between userId and scanner UserId 

                    OperationResult<TransactionDTO> transaction = _transactionService.Create(new TransactionDTO
                    {
                        ReceiveUserId = qRCodeDTO.UserCreatorId,
                        SendUserId = payer.scannerUserId,
                        QRCode = new QRCodeDTO
                        {
                            Id = qRCodeDTO.Id
                        }
                    });

                    if(transaction != null && transaction.Result == QueryResult.IsSucced) 
                    {
                        //Update qrCode

                        qRCodeDTO.UserScanerId = payer.scannerUserId;
                        OperationResult<QRCodeDTO> qrcode = _qrcodeService.Update(qRCodeDTO);
                        if(qrcode != null  && qrcode.Result == QueryResult.IsSucced) 
                        {
                            //Modifier WalletDTO balance
                            walletDto.Balance -= payer.Amount;
                            OperationResult<WalletDTO> updateWallet = _walletService.Update(walletDto);

                            if(updateWallet != null && updateWallet.Result == QueryResult.IsSucced) 
                            {
                                return new OperationResult<string>
                                {
                                    Result = QueryResult.IsSucced
                                };
                            }
                        }                        
                    }
                }               
            }
            return new OperationResult<string>
            {
                Result = QueryResult.IsFailed
            };
        }
    }
}
