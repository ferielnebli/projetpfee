using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Result
{
    public class OperationResult<T>
    {
        public QueryResult Result { get; set; }
        public string? ExceptionMessage { get; set; }
        public T? ObjectValue { get; set; }
    }
}
