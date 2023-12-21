using Pioneer.DAL.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Pioneer.Api.Dto
{
    public class Authdto
    {

        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public byte[]? PresonalPicture { get; set; }
        [JsonIgnore]
        public List<string> Roles { get; set; }
        public DateTime LogDatetime { get; set; } = DateTime.Now;
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }

        
    }
}
