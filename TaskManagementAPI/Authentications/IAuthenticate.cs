using TaskManagementAPI.Models;

namespace TaskManagementAPI.Authentications
{
    public interface IAuthenticate
    {
        Task<bool> IsUserAuthenticated(UserLogin credentials);
        int GetUserIdFromCredentials(UserLogin credentials);
        string HashPassword(string password);
    }
}
