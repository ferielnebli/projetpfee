using AutoMapper;
using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.BusinessModel;
using Dyno.Platform.Payment.DataModel;
using Dyno.Platform.Payment.DTO;
using Platform.Shared.Enum;
using Platform.Shared.Mapper;
using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Business.Services
{
    public class TransactionService : ITransactionService
    {
        public readonly IMapper _mapper;
        public readonly IMapperSession<TransactionEntity> _mapperSession;
        public TransactionService(IMapper mapper,
            IMapperSession<TransactionEntity> mapperSession)
        {
            _mapper = mapper;
            _mapperSession = mapperSession;
        }

        #region Get

        public OperationResult<List<TransactionDTO>> GetAll()
        {
            IList<TransactionEntity> transactionEntities = _mapperSession.GetAll();
            IList<Transaction> transactions = _mapper.Map<IList<Transaction>>(transactionEntities);
            IList<TransactionDTO> transactionDTOs = _mapper.Map<IList<TransactionDTO>>(transactions);
            return new OperationResult<List<TransactionDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = (List<TransactionDTO>)transactionDTOs
            };
        }

        public OperationResult<PagedList<TransactionDTO>> GetAll(PagedParameters pagedParameters)
        {
            OperationResult<List<TransactionDTO>> result = GetAll();
            return new OperationResult<PagedList<TransactionDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = PagedList<TransactionDTO>.ToGenericPagedList(result.ObjectValue, pagedParameters)
            };
        }

        public OperationResult<TransactionDTO> GetById(string id)
        {
            List<TransactionDTO>? transactionDTOs = GetAll().ObjectValue?.Where(x => x.Id == id).ToList();
            if (transactionDTOs != null && transactionDTOs.Count == 0)
            {
                return new OperationResult<TransactionDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"QRCode with this id : {id} not found"
                };
            }
            return new OperationResult<TransactionDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = transactionDTOs?.FirstOrDefault()
            };

        }
        public OperationResult<List<TransactionDTO>> Get(Func<TransactionDTO, bool> expression)
        {
            List<TransactionDTO>? transactionDTOs = GetAll().ObjectValue?.Where(expression).ToList();
            return new OperationResult<List<TransactionDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = transactionDTOs
            };
        }

        
        #endregion

        #region Create
        public OperationResult<TransactionDTO> Create(TransactionDTO dtoObject)
        {
            Transaction transaction = _mapper.Map<Transaction>(dtoObject);
            transaction.Id = Guid.NewGuid().ToString();
            TransactionEntity qrcodeEntity = _mapper.Map<TransactionEntity>(transaction);
            _mapperSession.Add(qrcodeEntity);
            return new OperationResult<TransactionDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Transaction Added successfully",
                ObjectValue= dtoObject
            };
        }
        #endregion

        #region Update

        public OperationResult<TransactionDTO> Update(TransactionDTO dtoObject)
        {
            Transaction transaction = _mapper.Map<Transaction>(dtoObject);
            TransactionEntity transactionEntity = _mapper.Map<TransactionEntity>(transaction);
            _mapperSession.Update(transactionEntity);
            return new OperationResult<TransactionDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Transaction Updated successfully",
                ObjectValue= dtoObject
            };
        }
        #endregion

        #region Delete
        public OperationResult<TransactionDTO> Delete(string id)
        {
            TransactionEntity transactionEntity = _mapperSession.GetById(new Guid(id));
            if (transactionEntity == null)
            {
                return new OperationResult<TransactionDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"Transaction with this id : {id} not found"
                };
            }
            _mapperSession.Delete(transactionEntity);
            return new OperationResult<TransactionDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Transaction deleted Successfully"
            };
        }
        #endregion
    }
}
