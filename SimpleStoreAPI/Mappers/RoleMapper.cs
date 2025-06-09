using SimpleStoreAPI.DTOs.Role;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Mappers
{
    public class RoleMapper
    {
        public static RoleDto ApplicationRoleToDto(ApplicationRole applicationRole)
        {
            return new RoleDto { Id=applicationRole.Id,  Name = applicationRole.Name!, Description = applicationRole.Description };
        }
    }
}
