using E_Com_Monolithic.Models;

namespace E_Com_Monolithic.Authentication.Jwt
{
    public interface ITokenManager
    {
        Task<string> CreateTokenAsync(User user);
    }
}
