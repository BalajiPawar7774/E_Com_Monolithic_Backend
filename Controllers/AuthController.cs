using AutoMapper;
using E_Com_Monolithic.Authentication;
using E_Com_Monolithic.Authentication.AuthRepositories;
using E_Com_Monolithic.Authentication.Jwt;
using E_Com_Monolithic.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Com_Monolithic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        public AuthController(IAuthRepository authRepository, IMapper mapper, ITokenManager tokenManager)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(UserDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            dto.PasswordHash = passwordHash;
            var user = _mapper.Map<Models.User>(dto);
            var registeredUser = await _authRepository.RegisterUser(user);

            if (registeredUser == null)
            {
                return BadRequest(new { success = false, message = "User registration failed!!."});
            }

            return Ok(new { success = true, message = "User Registration Successful !!", email = registeredUser.Email });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser(UserLoginDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _authRepository.GetUser(dto.Email);
            if(user == null)
            {
                return BadRequest("Invalid Email or Password");
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if(!isPasswordValid)
            {
                return BadRequest("Invalid Email or Password");
            }

            //token generation
            var token = await _tokenManager.CreateTokenAsync(user);

            //append token to cookie
            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // ← MUST BE FALSE for HTTP
                SameSite = SameSiteMode.None, // ← MUST BE None for cross-origin
                Expires = DateTime.UtcNow.AddHours(1),
                Path = "/"
            });

            var logedUserResponse = new LoggedInUserResponse
            {
                IsAuthenticated = true,
                Role = user.Role,
                UserId = user.UserId
            };
            return Ok(logedUserResponse);
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            // Clear the auth cookie
            Response.Cookies.Append("auth_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1), // Set to past date to delete
                Path = "/"
            });

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
