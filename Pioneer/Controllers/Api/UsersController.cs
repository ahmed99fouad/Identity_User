using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pioneer.Models;
using Pioneer.DAL.Model;

namespace Pioneer.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles ="Admin")]
    public class UsersController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

       
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("user not found");
            var result = await _userManager.DeleteAsync(user);
            //if (!result.Succeeded)
            //    throw new Exception();
            return Ok(result.Succeeded);

        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("role not found");
            var result = await _roleManager.DeleteAsync(role);
            //if (!result.Succeeded)
            //    throw new Exception();
            return Ok(result.Succeeded);

        }
    }
}
