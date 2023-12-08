using Dyno.Platform.ReferntialData.DataModel.UserData;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.BusinessModel.UserData
{
    public class UserOtp
    {
        public Guid Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Code { get; set; }
        public  Status Status { get; set; }
        public OtpType OtpType { get; set; }
    }
}
