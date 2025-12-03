using E_Com_Monolithic.Dtos;
using E_Com_Monolithic.Models;

namespace E_Com_Monolithic.Authentication.AuthRepositories
{
    public interface IAuthRepository
    {
        Task<User> RegisterUser(User user);
        Task<User?> GetUser(string email);
    }
}
