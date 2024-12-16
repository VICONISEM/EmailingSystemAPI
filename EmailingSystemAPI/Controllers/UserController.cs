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

            if (role == UserRoles.CollegeAdmin.ToString())
            {
                users = await unitOfWork.Repository<ApplicationUser>().GetAllQueryableWithSpecs(specs).Where(U => U.CollegeId == admin.CollegeId).ToListAsync();
                //count = await unitOfWork.Repository<ApplicationUser>.GetCountWithSpecs(specs);
                Count = users.Count;
            }
            else if(role == UserRoles.Admin.ToString())
            {
                users = await unitOfWork.Repository<ApplicationUser>().GetAllQueryableWithSpecs(specs).ToListAsync();
                //Count = await unitOfWork.Repository<ApplicationUser>.GetCountWithSpecs(specs);
                Count = users.Count;
            }

            var userDtoList = mapper.Map<List<UserDto>>(users);

            for(int i=0; i<users.Count(); i++) 
            {
                userDtoList[i].Role = (await userManager.GetRolesAsync(users[i])).ToString();
            }

            return Ok(new Pagination<UserDto>(Specs.PageNumber, Specs.PageSize, Count, userDtoList));
        }

        //Reset Password
        [HttpGet("ResetPassword/{userId}")]
        public async Task<ActionResult> ResetPassword(int userId)
        {
            var user = await userManager.FindByIdAsync();
            if (userId == 0) { return NotFound(new APIErrorResponse(401,"User Not Found.")); }

            if()

            return Ok();
        }


        //Edit Account
    }
}
