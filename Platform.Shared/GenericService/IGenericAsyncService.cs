﻿using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.GenericService
{
    public interface IGenericAsyncService<DTO, TypeId> where DTO: class
    {
        OperationResult<List<DTO>> GetAll();
        OperationResult<PagedList<DTO>> GetAll(PagedParameters pagedParameters);
        OperationResult<List<DTO>> Get(Func<DTO, bool> expression);
        Task<OperationResult<DTO>> GetById(TypeId id);
        Task<OperationResult<DTO>> Create(DTO entity);
        Task<OperationResult<DTO>> Update(DTO entity);
        Task<OperationResult<DTO>> Delete(TypeId id);

    }
}
