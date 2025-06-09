using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.DTOs.Role
{
    public class RoleResult
    {
        public RoleDto? RoleDto { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Errors = new List<string>();

        public static RoleResult Success()
        {
            return new RoleResult() { Succeeded = true };
        }
        public static RoleResult Success(RoleDto roleDto)
        {
            return new RoleResult() { Succeeded = true, RoleDto = roleDto };
        }

        public static RoleResult Failed(string error)
        {
            return new RoleResult { Succeeded = false, Errors = new List<string> { error } };
        }
        public static RoleResult Failed(List<string> errors)
        {
            return new RoleResult { Succeeded = false, Errors = errors };
        }
    }
}
