using AutoMapper;
using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.BusinessModel;
using Dyno.Platform.Payment.DataModel;
using Dyno.Platform.Payment.DTO;
using Platform.Shared.Enum;
using Platform.Shared.GenericService;
using Platform.Shared.Mapper;
using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Business.Services
{
    public class QRCodeService : IQRCodeService
    {
        public readonly IMapper _mapper;
        public readonly IMapperSession<QRCodeEntity> _mapperSession;
        public QRCodeService(IMapper mapper, 
            IMapperSession<QRCodeEntity> mapperSession) 
        {
            _mapper = mapper;
            _mapperSession = mapperSession;
        }

        #region Get
        public OperationResult<List<QRCodeDTO>> GetAll()
        {
            IList<QRCodeEntity> qrcodeEntities = _mapperSession.GetAll();
            IList<QRCode> qrcodes = _mapper.Map<IList<QRCode>>(qrcodeEntities);
            IList<QRCodeDTO> qRCodeDTOs = _mapper.Map<IList<QRCodeDTO>>(qrcodes);
            return new OperationResult<List<QRCodeDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = (List<QRCodeDTO>)qRCodeDTOs
            };
        }

        public OperationResult<PagedList<QRCodeDTO>> GetAll(PagedParameters pagedParameters)
        {
            OperationResult<List<QRCodeDTO>> result = GetAll();
            return new OperationResult<PagedList<QRCodeDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = PagedList<QRCodeDTO>.ToGenericPagedList(result.ObjectValue, pagedParameters)
            };
        }
        public OperationResult<List<QRCodeDTO>> Get(Func<QRCodeDTO, bool> expression)
        {
            List<QRCodeDTO>? qRCodeDTOs = GetAll().ObjectValue?.Where(expression).ToList();
            return new OperationResult<List<QRCodeDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = qRCodeDTOs
            };
        }

        public OperationResult<QRCodeDTO> GetById(string id)
        {
            List<QRCodeDTO>? qRCodeDTOs = GetAll().ObjectValue?.Where(x => x.Id == id).ToList();
            if(qRCodeDTOs != null && qRCodeDTOs.Count == 0)
            {
                return new OperationResult<QRCodeDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"QRCode with this id : {id} not found"
                };
            }
            return new OperationResult<QRCodeDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = qRCodeDTOs?.FirstOrDefault()
            };
        }
        #endregion

        #region Create
        public OperationResult<QRCodeDTO> Create(QRCodeDTO dtoObject)
        {
            QRCode qrcode = _mapper.Map<QRCode>(dtoObject);
            qrcode.Id = Guid.NewGuid().ToString();
            QRCodeEntity qrcodeEntity = _mapper.Map<QRCodeEntity>(qrcode);
            _mapperSession.Add(qrcodeEntity);
            return new OperationResult<QRCodeDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "QRCode Added successfully"
            };
        }
        #endregion

        #region Update
        public OperationResult<QRCodeDTO> Update(QRCodeDTO dtoObject)
        {
            QRCode qrcode = _mapper.Map<QRCode>(dtoObject);
            QRCodeEntity qrcodeEntity = _mapper.Map<QRCodeEntity>(qrcode);
            _mapperSession.Update(qrcodeEntity);
            return new OperationResult<QRCodeDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "QRCode Updated successfully"
            };
        }
        #endregion

        #region Delete
        public OperationResult<QRCodeDTO> Delete(string id)
        {
            QRCodeEntity qRCodeEntity = _mapperSession.GetById(new Guid(id));
            if (qRCodeEntity == null)
            {
                return new OperationResult<QRCodeDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"QRCode with this id : {id} not found"
                };
            }
            _mapperSession.Delete(qRCodeEntity);
            return new OperationResult<QRCodeDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "QRCode deleted Successfully"
            };
        }
        #endregion
    }
}
