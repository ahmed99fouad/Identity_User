using Microsoft.AspNetCore.Http;

namespace Pioneer.DAL.Dto
{
    public class EditProfileDto
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get;  set; }
        public string? LastName { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
