using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.UserSpecs;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystemAPI.DTOs;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<UserDto>>> GetAllUsers(UserSpecsParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByNameAsync(Email);
            var role = (await userManager.GetRolesAsync(admin)).ToString();

            var specs = new UserSpecifications(Specs, admin.Id);

            List<ApplicationUser> users = new List<ApplicationUser>();
            int Count = 0;

            if (role == UserRole.CollegeAdmin.ToString())
            {
                users = await unitOfWork.Repository<ApplicationUser>().GetAllQueryableWithSpecs(specs).Where(U => U.CollegeId == admin.CollegeId).ToListAsync();
                //count = await unitOfWork.Repository<ApplicationUser>.GetCountWithSpecs(specs);
                Count = users.Count;
            }
            else if (role == UserRole.Admin.ToString())
            {
                users = await unitOfWork.Repository<ApplicationUser>().GetAllQueryableWithSpecs(specs).ToListAsync();
                //Count = await unitOfWork.Repository<ApplicationUser>.GetCountWithSpecs(specs);
                Count = users.Count;
            }

            var userDtoList = mapper.Map<List<UserDto>>(users);

            for (int i = 0; i < users.Count(); i++)
            {
                userDtoList[i].Role = (await userManager.GetRolesAsync(users[i])).ToString();
            }

            return Ok(new Pagination<UserDto>(Specs.PageNumber, Specs.PageSize, Count, userDtoList));
        }

        //Reset Password
        [HttpGet("ResetPassword/{Email}")]
        public async Task<ActionResult> ResetPassword(string Email)
        {
            var adminEmail = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var adminRole = (await userManager.GetRolesAsync(admin)).ToString();

            var user = await userManager.FindByEmailAsync(Email);
            if (user is null) { return NotFound(new APIErrorResponse(401, "User Not Found.")); }

            if (adminRole == UserRole.CollegeAdmin.ToString())
            {
                if (user.CollegeId != admin.CollegeId)
                { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action for this user.")); }
            }


            await userManager.RemovePasswordAsync(user);
            var Result = await userManager.AddPasswordAsync(user,user.NationalId);

            if (!Result.Succeeded) return BadRequest(new APIErrorResponse(400, "An error ocurred, Please try again later."));

            return Ok();
        }

        //Edit Account
        [HttpPut("EditAccount")]
        public async Task<ActionResult> EditAccount(string Email, UserToUpdateDto userDto)
        {
            var adminEmail = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var adminRole = (await userManager.GetRolesAsync(admin)).ToString();

            var user = await userManager.FindByEmailAsync(Email);
            if (user is null) { return NotFound(new APIErrorResponse(401, "User Not Found.")); }

            if (adminRole == UserRole.CollegeAdmin.ToString())
            {
                if (user.CollegeId != admin.CollegeId)
                { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action for this user.")); }
            }

            //var appUser = mapper.Map<ApplicationUser>(userDto);

            var Result = userManager.UpdateAsync(/*appUser*/ user);

            if (!Result.IsCompletedSuccessfully) return BadRequest(new APIErrorResponse(401, "An error ocurred, Please try again later."));

            return Ok();
            
        }


        }
    }