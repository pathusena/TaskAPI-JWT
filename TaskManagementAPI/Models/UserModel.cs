namespace TaskManagementAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public bool Active { get; set; }

    }

    public class UserLogin {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
