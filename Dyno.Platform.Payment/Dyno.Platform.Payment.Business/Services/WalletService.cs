using AutoMapper;
using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.BusinessModel;
using Dyno.Platform.Payment.DataModel;
using Dyno.Platform.Payment.DTO;
using NBitcoin;
using Platform.Shared.Enum;
using Platform.Shared.Mapper;
using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Business.Services
{
    public class WalletService : IWalletService
    {
        public readonly IMapper _mapper;
        public readonly IMapperSession<WalletEntity> _mapperSession;
        public WalletService(IMapper mapper,
            IMapperSession<WalletEntity> mapperSession)
        {
            _mapper = mapper;
            _mapperSession = mapperSession;
        }

        #region Get
        public OperationResult<List<WalletDTO>> GetAll()
        {
            IList<WalletEntity> walletEntities = _mapperSession.GetAll();
            IList<Wallet> wallets = _mapper.Map<IList<Wallet>>(walletEntities);
            IList<WalletDTO> walletDTOs = _mapper.Map<IList<WalletDTO>>(wallets);
            return new OperationResult<List<WalletDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = (List<WalletDTO>)walletDTOs
            };
        }

        public OperationResult<PagedList<WalletDTO>> GetAll(PagedParameters pagedParameters)
        {
            OperationResult<List<WalletDTO>> result = GetAll();
            return new OperationResult<PagedList<WalletDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = PagedList<WalletDTO>.ToGenericPagedList(result.ObjectValue, pagedParameters)
            };
        }

        public OperationResult<WalletDTO> GetById(string id)
        {
            List<WalletDTO>? walletDTOs = GetAll().ObjectValue?.Where(x => x.Id == id).ToList();
            if (walletDTOs != null && walletDTOs.Count == 0)
            {
                return new OperationResult<WalletDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"QRCode with this id : {id} not found"
                };
            }
            return new OperationResult<WalletDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = walletDTOs?.FirstOrDefault()
            };
        }

        public OperationResult<List<WalletDTO>> Get(Func<WalletDTO, bool> expression)
        {
            List<WalletDTO>? walletDTOs = GetAll().ObjectValue?.Where(expression).ToList();
            return new OperationResult<List<WalletDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = walletDTOs
            };
        }
        #endregion

        #region Create
        public OperationResult<WalletDTO> Create(WalletDTO dtoObject)
        {
            Wallet wallet = _mapper.Map<Wallet>(dtoObject);
            wallet.Id = Guid.NewGuid().ToString();
            Dictionary<string, string> key = CreateKey();
            wallet.PublicAddress = key["PublicKey"];
            wallet.PrivateKey = key["PrivateKey"];
            wallet.Address = key["Address"];
            WalletEntity walletEntity = _mapper.Map<WalletEntity>(wallet);
            _mapperSession.Add(walletEntity);
            return new OperationResult<WalletDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Wallet Added successfully",
                ObjectValue = dtoObject
            };
        }
        #endregion

        #region Update
        public OperationResult<WalletDTO> Update(WalletDTO dtoObject)
        {
            Wallet wallet = _mapper.Map<Wallet>(dtoObject);
            WalletEntity walletEntity = _mapper.Map<WalletEntity>(wallet);
            _mapperSession.Update(walletEntity);
            return new OperationResult<WalletDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Wallet Updated successfully",
                ObjectValue = dtoObject
            };
        }
        #endregion

        #region Delete
        public OperationResult<WalletDTO> Delete(string id)
        {
            WalletEntity walletEntity = _mapperSession.GetById(new Guid(id));
            if (walletEntity == null)
            {
                return new OperationResult<WalletDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"Wallet with this id : {id} not found"
                };
            }
            _mapperSession.Delete(walletEntity);
            return new OperationResult<WalletDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Wallet deleted Successfully"
            };
        }
        #endregion


        private Dictionary<string, string> CreateKey()
        {
            Key privateKey = new Key();

            // Get the corresponding public key
            PubKey publicKey = privateKey.PubKey;

            // Get the Bitcoin address
            BitcoinAddress address = publicKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main);

            // Display the generated address, private key, and public key
            Console.WriteLine("Bitcoin Address: " + address);
            Console.WriteLine("Private Key: " + privateKey.GetWif(Network.Main));
            Console.WriteLine("Public Key: " + publicKey);

            return new Dictionary<string, string>
            {
                {"Address", address.ToString() },
                {"PrivateKey", privateKey.GetWif(Network.Main).ToString() },
                {"PublicKey", publicKey.ToString() },
            };

        }
    }
}
