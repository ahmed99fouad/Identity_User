using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pioneer.Contants;
using Pioneer.ViewModels;
using System.Security.Claims;

namespace Pioneer.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult>  Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", await _roleManager.Roles.ToListAsync());

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role is exists!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

            return RedirectToAction(nameof(Index));
        }
        //edit role
        //Edit
        public async Task<IActionResult> Editrole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound("role not fount");
            var viewModel = new RoleFormViewModel
            {
                Id = roleId,
                Name = role.Name,               
            };
            return View(viewModel);
        }

        //

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editrole(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
                return NotFound();
            var rolewithsameName = await _roleManager.FindByNameAsync(model.Name);
            if (rolewithsameName != null && rolewithsameName.Id != model.Id)
            {
                ModelState.AddModelError("Name", "Name is alrady exit");
                return View(model);
            }
            role.Name = model.Name;
            await _roleManager.UpdateAsync(role);
            return RedirectToAction(nameof(Index));
        }



        /// <summary>
        /// ///to add permission 
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound();

            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allClaims = Permissions.GenerateAllPermissions();
            var allPermissions = allClaims.Select(p => new RoleViewModel { RoleName = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.RoleName))
                    permission.IsSelected = true;
            }

            var viewModel = new PermissionsFormViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleCalims = allPermissions
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionsFormViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.RoleName));

            return RedirectToAction(nameof(Index));
        }


    }
}
