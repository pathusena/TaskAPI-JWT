using TaskManagementAPI.Models;

namespace TaskManagementAPI.Authentications
{
    public interface IAuthenticate
    {
        bool IsUserAuthenticated(UserCredentials credentials);
        string GetUserIdFromCredentials(UserCredentials credentials);
    }
}
