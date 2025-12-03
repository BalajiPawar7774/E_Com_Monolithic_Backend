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
            if(registeredUser == null)
            {
                return BadRequest("User registration failed!!.");
            }
            return Ok("User Registration Successfull !!");
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
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.password, user.PasswordHash);
            if(!isPasswordValid)
            {
                return BadRequest("Invalid Email or Password");
            }

            var logedUserResponse = new LoggedInUserResponse
            {
                IsAuthenticated = true,
                Role = user.Role,
                Token = _tokenManager.CreateTokenAsync(user).Result // Token generation logic can be added here
            };
            return Ok(logedUserResponse);
        }
    }
}
