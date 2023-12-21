namespace Pioneer.Api.Dto
{
    public class Userdto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public byte[]? NationalPicture { get; set; }
        public byte[]? PresonalPicture { get; set; }
        public DateTime LogDatetime { get; set; } = DateTime.Now;


    }
}
