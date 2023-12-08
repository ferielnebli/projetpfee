using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.EnvironmentVariable
{
    public static class JWTVariable
    {
        public static readonly string key = Environment.GetEnvironmentVariable("kEY");
        public static readonly string Issuer = Environment.GetEnvironmentVariable("Issuer");
        public static readonly string Audience = Environment.GetEnvironmentVariable("Audience");
        public static readonly string AccesTokenExpired = Environment.GetEnvironmentVariable("AccesTokenExpired");
        public static readonly string RefreshTokenExpired = Environment.GetEnvironmentVariable("RefreshTokenExpired");
        

    }
}
