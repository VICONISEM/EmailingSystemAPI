using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Entities.Token;
using EmailingSystem.Repository;
using EmailingSystemAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = await userManager.FindByEmailAsync(registerDto.Email);

            if (email is not null) return BadRequest();

            var AppUser = mapper.Map<ApplicationUser>(registerDto);

            await unitOfWork.Repository<ApplicationUser>().AddAsync(AppUser);

            var refreshToken = GenerateRefreshToken();

            var user = await userManager.FindByEmailAsync(registerDto.Email);

            user.RefreshTokens.Add(refreshToken);

            SetRefreshTokenInCookie(refreshToken.Token, DateTime.UtcNow.AddDays(double.Parse(configuration["JWT:DurationInDays"] ?? "30")));

            var userDto = mapper.Map<UserDto>(user);

            return Ok(userDto);
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

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookiewOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookiewOptions);
        }
    }
}
