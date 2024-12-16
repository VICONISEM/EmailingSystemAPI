using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
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

        public UserController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public async Task<ActionResult> GetAllUsers()
        //{
        //    var Email = User.FindFirstValue(ClaimTypes.Email);
        //    var admin = await userManager.FindByNameAsync(Email);
        //    var role = (await userManager.GetRolesAsync(admin)).ToString();


        //    if (role == UserRoles.CollegeAdmin.ToString())
        //    {
        //        var users = await unitOfWork.Repository<ApplicationUser>().GetAllQueryable().Where(U => U.CollegeId == admin.CollegeId).ToListAsync();
        //    }
        //    else

                
        //}

        //change Password
        //Edit Account
    }
}
