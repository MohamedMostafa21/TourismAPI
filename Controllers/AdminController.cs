using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourismAPI.DTO;
using TourismAPI.Models;

namespace TourismAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;


        public AdminController(UserManager<User> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
        }

        // Assign Role Endpoint
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {
            // Check if the user exists
            var user = await userManager.FindByNameAsync(assignRoleDto.UserName);
            if (user == null)
                return NotFound("User not found.");

            // Check if the role exists
            var roleExists = await roleManager.RoleExistsAsync(assignRoleDto.Role);
            if (!roleExists)
                return BadRequest($"Role '{assignRoleDto.Role}' does not exist.");

            // Check if the user has the role
            var isInRole = await userManager.IsInRoleAsync(user, assignRoleDto.Role);
            if (isInRole)
                return BadRequest("User already has this role.");

            var result = await userManager.AddToRoleAsync(user, assignRoleDto.Role);
            if (result.Succeeded)
            {
                return Ok($"Role '{assignRoleDto.Role}' assigned to user '{user.UserName}'.");
            }

            return BadRequest(result.Errors);
        }

        // Getting all user with thier Roles عشان نتفكر انهي ايه
        [HttpGet("GetAllUsersWithRoles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var users = userManager.Users.ToList();
            var userRoles = new List<object>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    Roles = roles
                });
            }

            return Ok(userRoles);
        }
    }
}
