using AutoMapper;
using Dyno.Platform.Payment.BusinessModel;
using Dyno.Platform.Payment.DataModel;
using Dyno.Platform.Payment.DTO;
using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Platform.Shared.Enum;
using Platform.Shared.Mapper;
using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Services.UserDataService
{
    public class UserTokenService : IUserTokenService
    {
        public readonly IMapper _mapper;
        public readonly IMapperSession<UserTokenEntity> _mapperSession;
        public UserTokenService(IMapper mapper,
            IMapperSession<UserTokenEntity> mapperSession)
        {
            _mapper = mapper;
            _mapperSession = mapperSession;
        }

        #region Get

        public OperationResult<List<UserTokenDTO>> GetAll()
        {
            IList<UserTokenEntity> userTokenEntities = _mapperSession.GetAll();
            IList<UserToken> userTokens = _mapper.Map<IList<UserToken>>(userTokenEntities);
            IList<UserTokenDTO> userTokenDTOs = _mapper.Map<IList<UserTokenDTO>>(userTokens);
            return new OperationResult<List<UserTokenDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = (List<UserTokenDTO>)userTokenDTOs
            };
        }

        public OperationResult<PagedList<UserTokenDTO>> GetAll(PagedParameters pagedParameters)
        {
            OperationResult<List<UserTokenDTO>> result = GetAll();
            return new OperationResult<PagedList<UserTokenDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = PagedList<UserTokenDTO>.ToGenericPagedList(result.ObjectValue, pagedParameters)
            };
        }

        public OperationResult<UserTokenDTO> GetById(string id)
        {
            List<UserTokenDTO>? userTokenDTOs = GetAll().ObjectValue?.Where(x => x.Id == id).ToList();
            if (userTokenDTOs != null && userTokenDTOs.Count == 0)
            {
                return new OperationResult<UserTokenDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"Token with this id : {id} not found"
                };
            }
            return new OperationResult<UserTokenDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = userTokenDTOs?.FirstOrDefault()
            };
        }

        public OperationResult<List<UserTokenDTO>> Get(Func<UserTokenDTO, bool> expression)
        {
            List<UserTokenDTO>? userTokenDTOs = GetAll().ObjectValue?.Where(expression).ToList();
            return new OperationResult<List<UserTokenDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = userTokenDTOs
            };
        }


        #endregion

        #region Create
        public OperationResult<UserTokenDTO> Create(UserTokenDTO dtoObject)
        {
            List<UserTokenDTO>? usertoken = Get(token => token.User.Id == dtoObject.User.Id).ObjectValue;
            if(usertoken != null && usertoken.Count > 0) 
            {
                return new OperationResult<UserTokenDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "User is already login !"
                };
            }
            UserToken userToken = _mapper.Map<UserToken>(dtoObject);
            userToken.Id = Guid.NewGuid().ToString();
            UserTokenEntity userTokenEntity = _mapper.Map<UserTokenEntity>(userToken);
            _mapperSession.Add(userTokenEntity);
            return new OperationResult<UserTokenDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Token Added successfully"
            };
        }
        #endregion

        #region Update
        public OperationResult<UserTokenDTO> Update(UserTokenDTO dtoObject)
        {
            UserToken userToken = _mapper.Map<UserToken>(dtoObject);
            UserTokenEntity userTokenEntity = _mapper.Map<UserTokenEntity>(userToken);
            _mapperSession.Update(userTokenEntity);
            return new OperationResult<UserTokenDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Token Updated successfully"
            };
        }
        #endregion

        #region Delete
        public OperationResult<UserTokenDTO> Delete(string id)
        {
            UserTokenEntity userTokenEntity = _mapperSession.GetById(new Guid(id));
            if (userTokenEntity == null)
            {
                return new OperationResult<UserTokenDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"Token with this id : {id} not found"
                };
            }
            _mapperSession.Delete(userTokenEntity);
            return new OperationResult<UserTokenDTO>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "Token deleted Successfully"
            };
        }
        #endregion

    }
}
