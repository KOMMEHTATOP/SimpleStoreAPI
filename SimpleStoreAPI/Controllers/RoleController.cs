using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleStoreAPI.DTOs.Role;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<RoleResult> CreateRole(CreateRoleDto createRoleDto)
        {

        }
    }
}
