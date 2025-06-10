using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleStoreAPI.DTOs.Role;
using SimpleStoreAPI.Interfaces;
using SimpleStoreAPI.Models;
using SimpleStoreAPI.Service;

namespace SimpleStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto roleDto)
        {
            var result = await _roleService.CreateAsync(roleDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetById), new {id = result.Data!.Id}, result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllAsync();
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _roleService.GetByIdAsync(id);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateRoleDto roleDto)
        {
            var result = await _roleService.UpdateAsync(id, roleDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleService.DeleteAsync(id);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            return NoContent();
        }
    }
}
