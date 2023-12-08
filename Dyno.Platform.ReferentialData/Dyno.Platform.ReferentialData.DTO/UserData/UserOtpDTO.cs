

using Platform.Shared.Enum;

namespace Dyno.Platform.ReferentialData.DTO.UserData
{
    public class UserOtpDTO
    {
        public  Guid Id { get; set; }
        public  string? PhoneNumber { get; set; }
        public  string? Code { get; set; }
        public Status Status { get; set; }

        public OtpType OtpType { get; set; }
    }
}
