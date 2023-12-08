using Dyno.Platform.ReferntialData.DataModel.UserData;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferntialData.DataModel.BalanceData
{
    public class BalanceEntity
    {
        public virtual Guid Id { get; set; }
        public virtual double Amount { get; set; }
        public virtual string Key { get; set; }
        public virtual Status Status { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
