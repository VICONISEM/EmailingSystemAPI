using AutoMapper;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.User;
using EmailingSystemAPI.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto ChangePasswordDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            var Result = await userManager.ChangePasswordAsync(user, ChangePasswordDto.OldPassword, ChangePasswordDto.NewPassword);

            if(!Result.Succeeded) return BadRequest(new APIErrorResponse(400,"Invalid Password"));

            return Ok(Result);
        }

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(Email);

            var userDto = mapper.Map<UserDto>(user);

            var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            userDto.Role = userRole;

            return Ok(userDto);
        }
        

    }
}
