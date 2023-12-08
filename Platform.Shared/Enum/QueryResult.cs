using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Enum
{
    public enum QueryResult
    {
        IsSucced=200, 
        IsFailed=400,
        UnAuthorized =401,
        InternalServerError = 500
    }
}
