using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Services.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Entities.Token;
using EmailingSystemAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = await userManager.FindByEmailAsync(registerDto.Email);

            if (email is not null) return BadRequest();

            var AppUser = mapper.Map<ApplicationUser>(registerDto);

            var Result = await userManager.CreateAsync(AppUser,registerDto.Password);

            if (!Result.Succeeded) return BadRequest();

            var refreshToken = GenerateRefreshToken();

            AppUser.RefreshTokens.Add(refreshToken);

            SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);

            var userDto = mapper.Map<AuthDto>(AppUser);

            userDto.AccessToken = await tokenService.CreateTokenAsync(AppUser,userManager);
            userDto.RefreshTokenExpirationTime = refreshToken.ExpiresOn;

            return Ok(userDto);
        }

        [HttpGet("LogIn")]
        public async Task<ActionResult<UserDto>> Login(LogInDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return BadRequest();

            var Result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if(!Result.Succeeded) return BadRequest();

            var userRefreshToken = user.RefreshTokens?.FirstOrDefault(T => T.IsActive)?.Token;

            if (string.IsNullOrEmpty(userRefreshToken))
            {
                var refreshToken = GenerateRefreshToken();
                SetRefreshTokenInCookie(refreshToken.Token, refreshToken.ExpiresOn);
            }

            var userDto = mapper.Map<AuthDto>(user);
            userDto.AccessToken = await tokenService.CreateTokenAsync(user,userManager);
            userDto.RefreshTokenExpirationTime = user.RefreshTokens.FirstOrDefault(T => T.IsActive).ExpiresOn;

            return Ok(userDto);
        }

        [HttpGet("LogOut")]
        public async Task<ActionResult> LogOut()
        {
            var refreshToken = Request?.Cookies["RefreshToken"];

            if (refreshToken is null) return NotFound();

            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            var TokenToDelete = user.RefreshTokens.FirstOrDefault(T => T.Token == refreshToken);

            var Result = user.RefreshTokens.Remove(TokenToDelete);

            if(!Result) return BadRequest();

            return Ok();
        }

        private RefreshToken GenerateRefreshToken()
        {
            var token = new Guid().ToString();

            return new RefreshToken
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"] ?? "30")),
                CreatedOn = DateTime.UtcNow
            };
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime ExpiresOn)
        {
            var cookiewOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = ExpiresOn.ToLocalTime()
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookiewOptions);
        }
    }
}
