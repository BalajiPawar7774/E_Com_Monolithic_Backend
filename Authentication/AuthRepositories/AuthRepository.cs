using E_Com_Monolithic.Dal;
using E_Com_Monolithic.Dtos;
using E_Com_Monolithic.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Com_Monolithic.Authentication.AuthRepositories
{    
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUser(string email)
        {
            var user = await _context.users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> RegisterUser(User user)
        {
            _context.users.Add(user);
            await  _context.SaveChangesAsync();
            return user;
        }
    }
}
