using Dyno.Platform.Payment.DTO;
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Dyno.Platform.ReferentialData.DTO.UserData
{
    public class UserProfileDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? Picture { get; set; }
        public double Balance { get; set; }

        public List<TransactionDTO>? Transactions { get; set; }
    }
}
