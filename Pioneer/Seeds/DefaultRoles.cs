//using Microsoft.AspNetCore.Identity;
//using Pioneer.Contants;

//namespace Pioneer.Seeds
//{
//    public class DefaultRoles
//    {
//        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
//        {
//            if (roleManager.Roles.Any())
//            {
//                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
//                await roleManager.CreateAsync(new IdentityRole(Roles.Merchant.ToString()));
//                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
//            }
//        }
//    }
//}
