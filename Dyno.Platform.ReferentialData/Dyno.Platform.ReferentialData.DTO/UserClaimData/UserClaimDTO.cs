using Dyno.Platform.ReferentialData.DTO.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using NHibernate.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.UserClaimData
{
    public class UserClaimDTO : IdentityUserClaim
    {
        //public virtual UserEntity User { get; set; }
        ////[NotMapped]
        ////public override string UserId { get; set; }
    }
}
