using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pioneer.Api.Dto;
using Pioneer.Api.Services;
using Pioneer.DAL;
using Pioneer.DAL.Dto;
using Pioneer.DAL.Model;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pioneer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthService _authService;



        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IAuthService authService)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _authService = authService;

        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] logindto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!_userManager.IsInRoleAsync(user, "Merchant").Result)
                return NotFound("this Account is not Authrized");

            //  Insert Log Of Merchnet User
            _dbContext.MerchantLogs.Add(new MerchantLog
            {
                ApplicationUserId = user.Id
            });

            _dbContext.SaveChanges();

            return Ok(result);
        }

        [Authorize(Roles = "Merchant")]
        [HttpPost("edit-info")]
        public async Task<IActionResult> EditProfileInfo([FromForm] EditProfileDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            // Get current User Id
            string id = User.Claims.Where(s => s.Type == "uid").First().Value;
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            if (!string.IsNullOrEmpty(model.Email))
            {
                var userwithsameEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userwithsameEmail != null && userwithsameEmail.Id != id)
                    return BadRequest("Email is alrady exit");

                user.Email = model.Email;
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                var userwithsameUserName = await _userManager.FindByNameAsync(model.UserName);
                if (userwithsameUserName != null && userwithsameUserName.Id != id)
                    return BadRequest("UserName is alrady exit");

                user.UserName = model.UserName;
            }

            if (!string.IsNullOrEmpty(model.FirstName))
                user.FirstName = model.FirstName;

            if (!string.IsNullOrEmpty(model.LastName))
                user.LastName = model.LastName;

            if (model.ProfileImage != null)
            {
                using (var dataStream = new MemoryStream())
                {
                    await model.ProfileImage.CopyToAsync(dataStream);
                    user.PresonalPicture = dataStream.ToArray();
                }
            }

            await _userManager.UpdateAsync(user);

            return Ok("Profile Updated");
        }


        [HttpGet("GetMarchants")]
        public IActionResult GetMarchants()
        {
            //var pageSize = int.Parse(Request.Form["length"]);
            //var skip = int.Parse(Request.Form["start"]);

            //var searchValue = Request.Form["search[value]"];

            //var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            //var sortColumnDirection = Request.Form["order[0][dir]"];

            //IQueryable<Customer> customers = _context.Customers.Where(m => string.IsNullOrEmpty(searchValue)
            //    ? true
            //    : (m.FirstName.Contains(searchValue) || m.LastName.Contains(searchValue) || m.Contact.Contains(searchValue) || m.Email.Contains(searchValue)));

            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            //    customers = customers.OrderBy(string.Concat(sortColumn, " ", sortColumnDirection));

            var marchant = _dbContext.MerchantLogs.Include(u => u.applicationUser)
                                                  .Select(m => new MerchantViewModel
                                                  {
                                                      LogDateTime = m.LogDatetime.ToString("yyyy-MM-dd hh:ss tt"),
                                                      FirstName = m.applicationUser.FirstName,
                                                      LastName = m.applicationUser.LastName,
                                                      Email = m.applicationUser.Email
                                                  }).ToList();
            //var data = MerchantLogs.Skip(skip).Take(pageSize).ToList();

            //var recordsTotal = marchant.Count();

            //var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data= marchant };

            return Ok(marchant);
        }



        //[HttpPost("login")]
        //public async Task<ActionResult<Userdto>> login(logindto logindtos)
        //{
        //    var user= await _userManager.FindByEmailAsync(logindtos.Email);
        //    if (user == null)
        //        return NotFound("this Account is not found");
        //    var resulte = await _signInManager.CheckPasswordSignInAsync(user, logindtos.Password ,false);
        //    if(!resulte.Succeeded)
        //        return NotFound("this Account is not found");

        //    //TODO
        //    // Confirm this claim user under Merchant Group Or Role
        //    if (_userManager.IsInRoleAsync(user, "SuperAdmin").Result)
        //        return NotFound("this Account is not found");




        //    //// Insert Log Of Merchnet User
        //    //_dbContext.MerchantLogs.Add(new MerchantLog
        //    //{
        //    //    ApplicationUserId = user.Id
        //    //});

        //    //_dbContext.SaveChanges();


        //    //return Ok(new Userdto()
        //    //{
        //    //    FirstName = user.FirstName,
        //    //    LastName = user.LastName,
        //    //    UserName = user.UserName,
        //    //    Email = user.Email,


        //    //});



    }
}
