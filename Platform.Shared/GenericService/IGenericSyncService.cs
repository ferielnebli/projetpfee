using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.GenericService
{
    public interface IGenericSyncService<DTO, TypeId>  where DTO: class
    {
        OperationResult<List<DTO>> GetAll();
        OperationResult<PagedList<DTO>> GetAll(PagedParameters pagedParameters);
        OperationResult<List<DTO>> Get(Func<DTO, bool> expression);
        OperationResult<DTO> GetById(TypeId id);
        OperationResult<DTO> Create(DTO dtoObject);
        OperationResult<DTO> Update(DTO dtoObject);
        OperationResult<DTO> Delete(TypeId id);
    }
}
