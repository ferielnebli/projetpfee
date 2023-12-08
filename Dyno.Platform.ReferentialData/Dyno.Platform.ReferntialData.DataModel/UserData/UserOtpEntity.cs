using Platform.Shared.Enum;

namespace Dyno.Platform.ReferntialData.DataModel.UserData
{
    public class UserOtpEntity
    {
        
        public virtual Guid Id {  get; set; }
        public virtual string? PhoneNumber{ get; set; }
        public virtual string? Code { get; set; }
        public virtual  Status  Status { get; set; }
        public virtual OtpType OtpType { get; set; }
    }
}
