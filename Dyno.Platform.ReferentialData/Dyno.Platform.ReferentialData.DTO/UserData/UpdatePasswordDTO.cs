using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.UserData
{
    public  class UpdatePasswordDTO
    {
        public string Id { get; set; } 
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
