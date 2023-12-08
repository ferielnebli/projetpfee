using Dyno.Platform.ReferentialData.DTO.AuthDTO;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Microsoft.AspNetCore.Identity;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Dyno.Platform.ReferentialData.Business.IServices.IAuthentification
{
    public interface IAuthentificationService
    {
        Task<OperationResult<LoginResponseDTO>> Login(LoginModelDTO loginModel);
        Task<OperationResult<UserProfileDTO>> Register(RegisterModelDTO register);   
        OperationResult<string> Logout(string userId);   
        Task<OperationResult<string>> ResetPassword(ResetPasswordDTO resetPassword);   
        Task<OperationResult<UserProfileDTO>> GetUserProfile(string  userId);   
        
    }
}
