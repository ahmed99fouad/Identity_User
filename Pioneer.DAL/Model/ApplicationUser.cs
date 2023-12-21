using Microsoft.AspNetCore.Identity;
using Pioneer.Api.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer.DAL.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public byte[]? ProfilePicture { get; set; }
        public byte[]? NationalPicture { get; set; }
        public byte[]? PresonalPicture { get; set; }

        public List<MerchantLog>? MerchantLogs { get; set; }







    }
}
