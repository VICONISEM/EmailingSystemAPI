using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.UserSpecs;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystem.Repository.Data.Contexts;
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
        private readonly EmailDbContext dbContext;

        public UserController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, EmailDbContext dbContext)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet("AllUsers")]
        public async Task<ActionResult<Pagination<UserDto>>> GetAllUsers(UserSpecsParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByNameAsync(Email);
            var role = (await userManager.GetRolesAsync(admin)).ToString();

            var specs = new UserSpecifications(Specs, admin);
            var CountSpecs = new UserSpecificationsForCountPagination(Specs, admin);

            List<ApplicationUser> users = await unitOfWork.Repository<ApplicationUser>().GetAllQueryableWithSpecs(specs).ToListAsync();
            int Count = await unitOfWork.Repository<ApplicationUser>().GetCountWithSpecs(CountSpecs);

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

            //Updating User
            var userRole = (await userManager.GetRolesAsync(user)).ToString();

            if (userRole != userDto.Role)
            {

                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var removeResult = await userManager.RemoveFromRoleAsync(user, userRole);

                        if (!removeResult.Succeeded)
                        {
                            throw new Exception($"Failed to remove user from role: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                        }

        
                        var newRole = Enum.Parse<UserRole>(userDto.Role, true).ToString();

                        var addResult = await userManager.AddToRoleAsync(user, newRole);

                        if (!addResult.Succeeded)
                        {
                            throw new Exception($"Failed to add user to role: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
                        }

                        // Commit transaction if everything succeeds
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        // Rollback transaction if any operation fails
                        await transaction.RollbackAsync();
                        throw; // Re-throw the exception to handle it at a higher level if needed
                    }
                }

            }

            user.Name = userDto.Name;
            user.NationalId = userDto.NationalId;
            user.CollegeId = userDto.CollegeId;
            user.DepartmentId = userDto.DepartmentId;

            //if(userDto.Picture != null)
            //{

            //}

            var Result = userManager.UpdateAsync(user);

            if (!Result.IsCompletedSuccessfully) return BadRequest(new APIErrorResponse(401, "An error ocurred, Please try again later."));

            return Ok();
            
        }

    }
}