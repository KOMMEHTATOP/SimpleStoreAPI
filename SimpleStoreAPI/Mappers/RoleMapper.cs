using SimpleStoreAPI.DTOs.Role;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Mappers
{
    public class RoleMapper
    {
        public static RoleResponseDto ApplicationRoleToRoleResponseDto(ApplicationRole applicationRole)
        {
            return new RoleResponseDto
            {
                Id = applicationRole.Id, 
                Name = applicationRole.Name!, 
                Description = applicationRole.Description
            };
        }
    }
}
