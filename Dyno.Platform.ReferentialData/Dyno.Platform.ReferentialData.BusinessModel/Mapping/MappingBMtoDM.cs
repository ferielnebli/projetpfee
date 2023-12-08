using AutoMapper;
using Dyno.Platform.ReferentialData.BusinessModel.UserClaimData;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferentialData.BusinessModel.UserRole;
using Dyno.Platform.ReferntialData.DataModel;
using Dyno.Platform.ReferntialData.DataModel.BalanceData;
using Dyno.Platform.ReferntialData.DataModel.UserClaim;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.BusinessModel.Mapping
{
    public class MappingBMtoDM : Profile
    {
        public MappingBMtoDM() {

            #region User Data
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<UserOtp, UserOtpEntity>().ReverseMap();
            CreateMap<UserClaim, UserClaimEntity>().ReverseMap();
            CreateMap<UserToken, UserTokenEntity>().ReverseMap();
            #endregion

            #region Role Data
            CreateMap<Role, RoleEntity>().ReverseMap();
            CreateMap<RoleClaim, RoleClaimEntity>().ReverseMap();
            #endregion

           
            

        }
    }
}
