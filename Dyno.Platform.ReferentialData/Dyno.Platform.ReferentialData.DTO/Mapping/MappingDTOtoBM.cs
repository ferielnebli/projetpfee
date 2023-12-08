using AutoMapper;
using Dyno.Platform.ReferentialData.BusinessModel;
using Dyno.Platform.ReferentialData.BusinessModel.UserClaimData;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferentialData.BusinessModel.UserRole;
using Dyno.Platform.ReferentialData.DTO.AuthDTO;
using Dyno.Platform.ReferentialData.DTO.RoleData;

using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using Dyno.Platform.ReferentialData.DTO.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.Mapping
{
    public class MappingDTOtoBM : Profile
    {
        public MappingDTOtoBM() 
        {
            #region User Data
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserClaimDTO, UserClaim>().ReverseMap();
            CreateMap<UpdateUserDTO, User>().ReverseMap();
            CreateMap<UserOtpDTO, UserOtp>().ReverseMap();
            CreateMap<RegisterModelDTO, User>().ReverseMap();
            CreateMap<UserTokenDTO, UserToken>().ReverseMap();
            #endregion

            #region Role Data
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<RoleClaimDTO, RoleClaim>().ReverseMap();
            #endregion

            

        }
    }
}
