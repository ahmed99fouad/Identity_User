using Pioneer.Api.Dto;

namespace Pioneer.Api.Services
{
    public interface IAuthService
    {
        //Task<Authdto> RegisterAsync(RegisterModel model);
        Task<Authdto> GetTokenAsync(logindto model);
        //Task<string> AddRoleAsync(AddRoleModel model);
    }
}
