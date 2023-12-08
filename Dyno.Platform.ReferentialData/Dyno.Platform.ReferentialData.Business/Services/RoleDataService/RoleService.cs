using AutoMapper;
using Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService;
using Dyno.Platform.ReferentialData.BusinessModel.UserRole;

using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferentialData.Nhibernate;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using Microsoft.AspNetCore.Identity;
using NHibernate.Id.Insert;
using Platform.Shared.Enum;
using Platform.Shared.GenericService;
using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Services.RoleDataService
{
    public class RoleService : IRoleService
    {
        public readonly IMapper _mapper;
        public readonly RoleManager<RoleEntity> _roleManager;
        public readonly UserManager<UserEntity> _userManager;

        public RoleService(IMapper mapper, 
            RoleManager<RoleEntity> roleManager, 
            UserManager<UserEntity> userManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Get
        public OperationResult<List<RoleDTO>> GetAll()
        {
            IList<RoleEntity> roleEntities = _roleManager.Roles.ToList();
            foreach (var role in roleEntities)
            {
                foreach (var user in role.Users) { user.Roles = null; }

            }
            IList<Role> roles = _mapper.Map<IList<Role>>(roleEntities);
            IList<RoleDTO> roleDTOs = _mapper.Map<IList<RoleDTO>>(roles);
            return new OperationResult<List<RoleDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = (List<RoleDTO>)roleDTOs
            };
        }

        public OperationResult<PagedList<RoleDTO>> GetAll(PagedParameters pagedParameters)
        {
            IList<RoleEntity> roleEntities = _roleManager.Roles.ToList();
            foreach (var role in roleEntities)
            {
                foreach (var user in role.Users) { user.Roles = null; }

            }
            IList<Role> roles = _mapper.Map<IList<Role>>(roleEntities);
            IList<RoleDTO> roleDTOs = _mapper.Map<IList<RoleDTO>>(roles);
            return new OperationResult<PagedList<RoleDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = PagedList<RoleDTO>.ToGenericPagedList(roleDTOs, pagedParameters)
            };
        }

        public OperationResult<List<RoleDTO>> Get(Func<RoleDTO, bool> expression)
        {
            List<RoleDTO>? roleDTOs = GetAll().ObjectValue?.Where(expression).ToList();
            return new OperationResult<List<RoleDTO>>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = roleDTOs
            };
        }

        public async Task<OperationResult<RoleDTO>> GetById(string id)
        {
            RoleEntity? roleEntity = await _roleManager.FindByIdAsync(id);
            if (roleEntity == null)
            {
                return new OperationResult<RoleDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"Role with id : {id} not found"
                };
            }
            Role role = _mapper.Map<Role>(roleEntity);
            RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);
            return new OperationResult<RoleDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = roleDTO
            };
        }

        public async Task<OperationResult<RoleDTO>> GetByName(string name)
        {
            RoleEntity? roleEntity = await _roleManager.FindByNameAsync(name);
            if (roleEntity == null)
            {
                return new OperationResult<RoleDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = $"Role with this name : {name} not found"
                };
            }
            Role role = _mapper.Map<Role>(roleEntity);
            RoleDTO roleDTO = _mapper.Map<RoleDTO>(role);
            return new OperationResult<RoleDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = roleDTO
            };
        }
        #endregion

        #region Create
        public async Task<OperationResult<RoleDTO>> Create(RoleDTO entity)
        {
            Role role = _mapper.Map<Role>(entity);
            role.Id = Guid.NewGuid().ToString();
            role.ConcurrencyStamp = DateTime.Now.ToString();
            RoleEntity roleEntity = _mapper.Map<RoleEntity>(role);
            var result = await _roleManager.CreateAsync(roleEntity);
            return new OperationResult<RoleDTO>
            {
                Result = result.Succeeded ? QueryResult.IsSucced : QueryResult.IsFailed,
                ExceptionMessage = !result.Succeeded ? result.Errors.ToList()[0].Description : "Role created successfully",
                ObjectValue = entity

            };
        }
        #endregion

        #region Update
        public async Task<OperationResult<RoleDTO>> Update(RoleDTO entity)
        {
            Role role = _mapper.Map<Role>(entity);
            RoleEntity roleEntity = _mapper.Map<RoleEntity>(role);
            var result = await _roleManager.UpdateAsync(roleEntity);
            return new OperationResult<RoleDTO>
            {
                Result = result.Succeeded ? QueryResult.IsSucced : QueryResult.IsFailed,
                ExceptionMessage = !result.Succeeded ? result.Errors.ToList()[0].Description : "Role uodate successfully",
                ObjectValue = entity
            };
        }
        #endregion

        #region Delete
        public async Task<OperationResult<RoleDTO>> Delete(string id)
        {

            RoleEntity? roleEntity = await _roleManager.FindByIdAsync(id);
            if(roleEntity == null)
            {
                return new OperationResult<RoleDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "Role Unfound"
                };
            }
            var result = await _roleManager.DeleteAsync(roleEntity);
            RoleDTO roleDTO = new RoleDTO { };
            if(result.Succeeded)
            {
                Role role = _mapper.Map<Role>(roleEntity);
                roleDTO = _mapper.Map<RoleDTO>(role);
            }
            return new OperationResult<RoleDTO>
            {
                Result = result.Succeeded ? QueryResult.IsSucced : QueryResult.IsFailed,
                ExceptionMessage = !result.Succeeded ? result.Errors.ToList()[0].Description : "Role deleted successfully",
                ObjectValue = roleDTO
            };
        }
        #endregion
    }
}
