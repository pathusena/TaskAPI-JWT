using TaskManagementAPI.Models;

namespace TaskManagementAPI.Authentications
{
    public interface IAuthenticate
    {
        Task<bool> IsUserAuthenticated(UserModel credentials);
        int GetUserIdFromCredentials(UserModel credentials);
    }
}
