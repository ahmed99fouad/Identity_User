﻿//using Microsoft.AspNetCore.Identity;
//using Pioneer.Contants;
//using Pioneer.Models;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Pioneer.Seeds
//{
//    public static class DefaultUsers
//    {


//        //private readonly UserManager<ApplicationUser> _userManager;
//        //private readonly RoleManager<IdentityRole> _roleManager;
//        //public DefaultUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        //{
//        //    _userManager = userManager;
//        //    _roleManager = roleManager;
//        //}

         
//        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            var defaultUser = new ApplicationUser
//            {
//                UserName = "basicuser@domain.com",
//                Email = "basicuser@domain.com",
//                EmailConfirmed = true
//            };

//            var user = await userManager.FindByEmailAsync(defaultUser.Email);

//            if (user == null)
//            {
//                await userManager.CreateAsync(defaultUser, "P@ssword123");
//                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
//            }
//        }

//        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            var defaultUser = new ApplicationUser
//            {
//                UserName = "superadmin@domain.com",
//                Email = "superadmin@domain.com",
//                EmailConfirmed = true
//            };

//            var user = await userManager.FindByEmailAsync(defaultUser.Email);

//            if (user == null)
//            {
//                await userManager.CreateAsync(defaultUser, "P@ssword123");
//                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.Basic.ToString(), Roles.Merchant.ToString(), Roles.SuperAdmin.ToString() });
//            }

//            await roleManager.SeedClaimsForSuperUser();
//        }

//        private static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
//        {
//            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
//            await roleManager.AddPermissionClaims(adminRole, "Products");
//        }

//        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
//        {
//            var allClaims = await roleManager.GetClaimsAsync(role);
//            var allPermissions = Permissions.GeneratePermissionsList(module);

//            foreach (var permission in allPermissions)
//            {
//                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
//                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
//            }
//        }
//    }
//}