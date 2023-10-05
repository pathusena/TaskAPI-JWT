using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Authentications
{
    public class Authenticate : IAuthenticate
    {
        private readonly ApplicationDBContext _context; // Inject your DbContext

        public Authenticate(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserAuthenticated(UserLogin credentials)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == credentials.Username && u.Active == true);

            if (user == null)
            {
                return false;
            }

            credentials.Id = user.Id;

            bool isPasswordValid = VerifyPassword(user, credentials.Password);

            return isPasswordValid;
        }

        public int GetUserIdFromCredentials(UserLogin credentials)
        {
            return credentials.Id;
        }

        private bool VerifyPassword(UserModel user, string inputPassword)
        {
            var hashedPassword = HashPassword(inputPassword);
            if (user.PasswordHash == hashedPassword) { 
                return true;
            }
            return false;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToBase64String(bytes);
        }
    }
}
