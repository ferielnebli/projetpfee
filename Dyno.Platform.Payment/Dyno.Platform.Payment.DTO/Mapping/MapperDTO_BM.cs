using AutoMapper;
using Dyno.Platform.Payment.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.DTO.Mapping
{
    public class MapperDTO_BM : Profile
    {
        public MapperDTO_BM() 
        {
            CreateMap<QRCodeDTO, QRCode>().ReverseMap();
            CreateMap<TransactionDTO, Transaction>().ReverseMap();
            CreateMap<WalletDTO, Wallet>().ReverseMap();
        }
    }
}
