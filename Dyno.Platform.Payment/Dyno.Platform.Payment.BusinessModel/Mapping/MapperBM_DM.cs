using AutoMapper;
using Dyno.Platform.Payment.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.BusinessModel.Mapping
{
    public class MapperBM_DM : Profile
    {
        public MapperBM_DM() 
        { 
            CreateMap<QRCode, QRCodeEntity>().ReverseMap();
            CreateMap<Transaction, TransactionEntity>().ReverseMap();
            CreateMap<Wallet, WalletEntity>().ReverseMap();
        }
    }
}
