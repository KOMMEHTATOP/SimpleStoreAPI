using Microsoft.AspNetCore.Mvc;
using SimpleStoreAPI.DTOs.User;
using SimpleStoreAPI.Interfaces;

namespace SimpleStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (!user.Succeeded)
            {
                return NotFound(user.Errors);    
            }
            return Ok(user.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserDto userDto)
        {
            var result = await _userService.UpdateAsync(id, userDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            return NoContent();
        }
    }
}


