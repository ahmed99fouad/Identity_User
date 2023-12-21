using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Pioneer.DAL.Model;
using Pioneer.Models;
using Pioneer.ViewModels;

namespace Pioneer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult>  Index()
        {
            //to return all user from database
            var users = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result,
                PresonalPicture = user.PresonalPicture,
                NationalPicture = user.NationalPicture,
            }).ToListAsync();

            return View(users);
        }


        //to add new user
        public async Task<IActionResult> Add(string userId)
        {
            

            var roles = await _roleManager.Roles.Select(r=>new RoleViewModel { RoleId=r.Id,RoleName=r.Name}).ToListAsync();

            var viewModel = new AddUserViewModel
            {

                Roles = roles
            };

            return View(viewModel);
        }
        //post action for add 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles","pless select one role");
                return View(model);

            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is alrady exit");
                return View(model);

            }

            if (await _userManager.FindByNameAsync(model.UserName) !=null)
            {
                ModelState.AddModelError("UserName", "UserName is alrady exit");
                return View(model);

            }

            var user = new ApplicationUser {
                UserName = model.UserName,
                Email=model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PresonalPicture = model.PresonalPicture,
                NationalPicture = model.NationalPicture,
                EmailConfirmed = true

            };        
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);

            }
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            var PresonalPicture = Request.Form.Files["PresonalPicture"];
            if (PresonalPicture != null)
            {

                //check file size and extension

                using (var dataStream = new MemoryStream())
                {
                    await PresonalPicture.CopyToAsync(dataStream);
                    user.PresonalPicture = dataStream.ToArray();
                }

                await _userManager.UpdateAsync(user);
            }

            var NationalPicture = Request.Form.Files["NationalPicture"];
            if (NationalPicture != null)
            {
                //check file size and extension

                using (var dataStream = new MemoryStream())
                {
                    await NationalPicture.CopyToAsync(dataStream);
                    user.NationalPicture = dataStream.ToArray();
                }

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        //Edit
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("user not fount");


            var viewModel = new ProfileFormViewModel
            {
                Id=userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PresonalPicture = user.PresonalPicture,
                NationalPicture = user.NationalPicture,

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            var userwithsameEmail = await _userManager.FindByEmailAsync(model.Email);
            if(userwithsameEmail != null && userwithsameEmail.Id!=model.Id)
            {
                ModelState.AddModelError("Email", "Email is alrady exit");
                return View(model);

            }
            var userwithsameUserName = await _userManager.FindByNameAsync(model.UserName);
            if (userwithsameUserName != null && userwithsameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "UserName is alrady exit");
                return View(model);

            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            //user.PresonalPicture = model.PresonalPicture;
            //user.NationalPicture = model.NationalPicture;

            var PresonalPicture = Request.Form.Files["PresonalPicture2"];
            if (PresonalPicture != null)
            {

                //check file size and extension

                using (var dataStream = new MemoryStream())
                {
                    await PresonalPicture.CopyToAsync(dataStream);
                    user.PresonalPicture = dataStream.ToArray();
                }

            }

            var NationalPicture = Request.Form.Files["NationalPicture2"];
            if (NationalPicture != null)
            {
                //check file size and extension

                using (var dataStream = new MemoryStream())
                {
                    await NationalPicture.CopyToAsync(dataStream);
                    user.NationalPicture = dataStream.ToArray();
                }
            }


            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));
        }

       



    }
}
