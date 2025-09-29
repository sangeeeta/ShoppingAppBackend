namespace ShoppingApp.Model
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // should hash in real project
        public string Role { get; set; } = "User";
    }
}
