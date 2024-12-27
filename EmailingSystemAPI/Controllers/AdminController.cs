using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.UserSpecs;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystem.Repository.Data.Contexts;
using EmailingSystem.Services;
using EmailingSystemAPI.DTOs.User;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,CollegeAdmin")]

    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly EmailDbContext dbContext;

        public AdminController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, EmailDbContext dbContext)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet("AllUsers")]
        public async Task<ActionResult<Pagination<UserDto>>> GetAllUsers([FromQuery]UserSpecsParams Specs)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(Email);
            var role = (await userManager.GetRolesAsync(admin)).FirstOrDefault();

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
            var adminRole = (await userManager.GetRolesAsync(admin)).FirstOrDefault();

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




            #region Some Validations

            if (userDto.Role == UserRole.Admin)
            {
                return Unauthorized(new APIErrorResponse(401, "Can't create user with Role Admin."));
            }
            else if (userDto.Role == UserRole.CollegeAdmin)
            {
                if (userDto.CollegeId is null || userDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "User with Role College Admin must be in a college and can't be in a department."));
            }
            else if (userDto.Role == UserRole.Presedient)
            {
                if (userDto.CollegeId.HasValue || userDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "Can't create user with Role Presedient with department and college."));
            }
            else if (userDto.Role == UserRole.VicePresedientForStudentsAffairs || userDto.Role == UserRole.VicePresedientForEnvironment || userDto.Role == UserRole.VicePresedientForPostgraduatStudies)
            {
                if (userDto.CollegeId.HasValue || userDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "Can't create user with Role VicePresedient with department and college."));
            }
            else if (userDto.Role == UserRole.Dean)
            {
                if (userDto.CollegeId is null || userDto.DepartmentId.HasValue)
                    return BadRequest("User with Role Dean must have a college and can't be in a department.");
            }
            else if (userDto.Role == UserRole.ViceDeanForStudentsAffairs || userDto.Role == UserRole.ViceDeanForEnvironment || userDto.Role == UserRole.ViceDeanForPostgraduatStudies)
            {
                if (!userDto.CollegeId.HasValue || userDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "Can't create user with Role ViceDean with department."));
            }
            else if (userDto.Role == UserRole.Secretary)
            {
                if (userDto.CollegeId is null || userDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "User with Role Secertary must be in a college and can't be in  a department."));
            }
            else if (userDto.Role == UserRole.NormalUser)
            {
                if (!userDto.CollegeId.HasValue || !userDto.DepartmentId.HasValue)
                    return BadRequest(new APIErrorResponse(400, "User with Role Normal User must be in a college and in a depertment."));
            }


            //Check if the selected department in the selected college
            if (userDto.DepartmentId is not null && userDto.CollegeId is not null)
            {
                var College = await unitOfWork.Repository<College>().GetByIdAsync<int>(userDto.CollegeId);
                var Department = await unitOfWork.Repository<Department>().GetByIdAsync<int>(userDto.DepartmentId);

                if (Department is null) return NotFound(new APIErrorResponse(404, "Department Not Found."));
                if (College is null) return NotFound(new APIErrorResponse(404, "College Not Found."));

                if (!College.Departments.Any(D => D.Id == userDto.DepartmentId))
                    return BadRequest(new APIErrorResponse(400, "This College doesn't contain such a department"));
            }

            #endregion










            //Updating User
            var Role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var userRole = (UserRole)Enum.Parse(typeof(UserRole), Role);

            if (userRole != userDto.Role)
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var removeResult = await userManager.RemoveFromRoleAsync(user, userRole.ToString());

                        if (!removeResult.Succeeded)
                        {
                            throw new Exception($"Failed to remove user from role: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                        }

                        var newRole = Enum.Parse<UserRole>(userDto.Role.ToString(), true).ToString();

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

            if(userDto.Picture != null)
            {
                await FileHandler.DeleteFile(user.PicturePath);
                user.PicturePath = await FileHandler.SaveFile(userDto.Picture.FileName, "ProfileImages",userDto.Picture);
            }

            if (userDto.Signature != null)
            {
                await FileHandler.DeleteFile(user.Signature.FilePath);
                user.Signature.FilePath = await FileHandler.SaveFile(userDto.Signature.FileName, "Signatures", userDto.Signature);
            }

            var Result = await userManager.UpdateAsync(user);

            if (!Result.Succeeded) return BadRequest(new APIErrorResponse(400, "An error ocurred, Please try again later."));

            return Ok();   
        }

        //Get Account By Email
        [HttpGet("GetAccountByEmail")]
        public async Task<ActionResult<UserDto>> GetAccountByEmail(string Email)
        {
            var adminEmail = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var adminRole = (await userManager.GetRolesAsync(admin)).FirstOrDefault();

            var user = await userManager.FindByEmailAsync(Email);
            if (user is null) { return NotFound(new APIErrorResponse(401, "User Not Found.")); }

            if (adminRole == UserRole.CollegeAdmin.ToString())
            {
                if (user.CollegeId != admin.CollegeId)
                { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action for this user.")); }
            }

            var userDto = mapper.Map<UserDto>(user);

            userDto.Role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            return Ok(userDto);
        }

        ////Soft Delete
        //[HttpDelete]
        //public async Task<ActionResult> DeleteAccount(string Email)
        //{
        //    var adminEmail = User.FindFirstValue(ClaimTypes.Email);
        //    var admin = await userManager.FindByEmailAsync(adminEmail);
        //    var adminRole = (await userManager.GetRolesAsync(admin)).ToString();

        //    var user = await userManager.FindByEmailAsync(Email);
        //    if (user is null) { return NotFound(new APIErrorResponse(401, "User Not Found.")); }

        //    if (adminRole == UserRole.CollegeAdmin.ToString())
        //    {
        //        if (user.CollegeId != admin.CollegeId)
        //        { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action for this user.")); }
        //    }

        //    user.IsDeleted = true;
        //    await userManager.UpdateAsync(user);

        //    return Ok(user);
        //}
    }
}