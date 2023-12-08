using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.EnvironmentVariable
{
    public static class TwilioVariable
    {
        public static readonly string AccountSID = Environment.GetEnvironmentVariable("AccountSID");
        public static readonly string AuthToken = Environment.GetEnvironmentVariable("AuthToken");
        public static readonly string TwilioPhone = Environment.GetEnvironmentVariable("TwilioPhoneNumber");
    }
}
