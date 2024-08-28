using Core.DTO.RolesDtos;

namespace Core.ServiceContracts
{
    public interface IRolesServices
    {
        public Task AddRole(AddRoleDto addRoleDto);

        public Task AddUserToRole(AddUserToRoleDto addUserToRoleDto);

        public Task DeleteRole(DeleteRoleDto deleteRoleDto);

        public Task<IList<string>> GetUserRoles(GetUserRolesDto getUserRolesDto);

        public Task RemoveUserFromRole(RemoveUserFromRoleDto removeUserFromRoleDto);

        public Task UpdateRoleName(UpdateRoleNameDto updateRoleNameDto);
    }
}
