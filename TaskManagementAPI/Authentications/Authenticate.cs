using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> IsUserAuthenticated(UserModel credentials)
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

        public int GetUserIdFromCredentials(UserModel credentials)
        {
            return credentials.Id;
        }

        private bool VerifyPassword(UserModel user, string inputPassword)
        {
            if (user.Password.Equals(inputPassword)) { 
                return true;
            }
            return false;
        }
    }
}
