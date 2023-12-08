using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.HttpHelper
{
    public interface IHelper<DTO>
    {
        #region Get Without autorization
        public OperationResult<DTO>? Get(string endPoint, string id);
        public OperationResult<List<DTO>>? GetAll(string endPoint);
        #endregion

        #region Post
        public OperationResult<DTO>? Post(string endPoint, DTO element);
        #endregion

        #region Patch
        public OperationResult<DTO>? Patch(string endPoint, DTO element);
        #endregion

        #region Put
        public OperationResult<DTO>? Put(string endPoint, DTO element);
        #endregion
    }
}
