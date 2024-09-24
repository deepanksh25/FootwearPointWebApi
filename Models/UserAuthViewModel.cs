namespace FootwearPointWebApi.Models
{
    public class UserAuthViewModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Surname { get; set; }
        public string token { get; set; } = "";
    }
}
