using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pioneer.Api.Dto;
using Pioneer.Api.Helpers;
using Pioneer.DAL.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pioneer.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        //public async Task<Authdto> RegisterAsync(RegisterModel model)
        //{
        //    if (await _userManager.FindByEmailAsync(model.Email) is not null)
        //        return new Authdto { Message = "Email is already registered!" };

        //    if (await _userManager.FindByNameAsync(model.Username) is not null)
        //        return new Authdto { Message = "Username is already registered!" };

        //    var user = new ApplicationUser
        //    {
        //        UserName = model.Username,
        //        Email = model.Email,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName
        //    };

        //    var result = await _userManager.CreateAsync(user, model.Password);

        //    if (!result.Succeeded)
        //    {
        //        var errors = string.Empty;

        //        foreach (var error in result.Errors)
        //            errors += $"{error.Description},";

        //        return new Authdto { Message = errors };
        //    }

        //    await _userManager.AddToRoleAsync(user, "User");

        //    var jwtSecurityToken = await CreateJwtToken(user);

        //    return new Authdto
        //    {
        //        Email = user.Email,
        //        ExpiresOn = jwtSecurityToken.ValidTo,
        //        IsAuthenticated = true,
        //        Roles = new List<string> { "User" },
        //        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
        //        Username = user.UserName
        //    };
        //}

        public async Task<Authdto> GetTokenAsync(logindto model)
        {
            var Authdto = new Authdto();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                Authdto.Message = "Email or Password is incorrect!";
                return Authdto;
            }
           



            var rolesList = await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);

            Authdto.IsAuthenticated = true;
            Authdto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            Authdto.Email = user.Email;
            Authdto.Username = user.UserName;
            Authdto.LogDatetime =DateTime.Now;
            Authdto.ExpiresOn = jwtSecurityToken.ValidTo;
            Authdto.Roles = rolesList.ToList();

            
               


                return Authdto;
        }

        //public async Task<string> AddRoleAsync(AddRoleModel model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);

        //    if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
        //        return "Invalid user ID or Role";

        //    if (await _userManager.IsInRoleAsync(user, model.Role))
        //        return "User already assigned to this role";

        //    var result = await _userManager.AddToRoleAsync(user, model.Role);

        //    return result.Succeeded ? string.Empty : "Sonething went wrong";
        //}

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
