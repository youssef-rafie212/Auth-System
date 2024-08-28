using Core.Domain.Entities;

namespace Core.Helpers
{
    public class ValidationHelpers
    {
        // This method throws an exception if the given tenant id doesn't match the tenant id of the given user 
        // (which means that this user doesn't belong to the current client)
        public void ThrowIfUnmatchedUserTenantId(string tenantId, ApplicationUser user)
        {
            if (tenantId != user.TenantId) throw new UnauthorizedAccessException("This user doesn't belong to the current client.");
        }

        // This method throws an exception if the given tenant id doesn't match the tenant id of the given role 
        // (which means that this role doesn't belong to the current client)
        public void ThrowIfUnmatchedRoleTenantId(string tenantId, ApplicationRole role)
        {
            if (tenantId != role.TenantId) throw new UnauthorizedAccessException("This role doesn't belong to the current client.");
        }
    }
}
