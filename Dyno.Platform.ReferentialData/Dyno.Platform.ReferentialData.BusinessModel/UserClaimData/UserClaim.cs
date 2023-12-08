using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using NHibernate.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.BusinessModel.UserClaimData
{
    public class UserClaim : IdentityUserClaim
    {
        //public virtual UserEntity User { get; set; }
        //[NotMapped]
        //public override string UserId { get; set; }
    }
}
