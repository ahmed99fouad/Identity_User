using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pioneer.DAL.Model
{
    public class MerchantLog
    {

        [Key]
        public int Id { get; set; }
        public DateTime LogDatetime { get; set; } = DateTime.Now;


        public string ApplicationUserId { get; set; }


        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser applicationUser { get; set; }

    }
}
