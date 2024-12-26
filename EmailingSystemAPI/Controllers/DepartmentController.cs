using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using EmailingSystem.Repository.Data.Contexts;
using EmailingSystemAPI.DTOs.Department;
using EmailingSystemAPI.Errors;
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
    public class DepartmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly EmailDbContext dbContext;

        public DepartmentController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, EmailDbContext dbContext)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }

        [HttpPost("AddDepartment")]
        public async Task<ActionResult> AddDepartment(DepartmentDto departmentDto)
        {
            var adminEmail = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var adminRole = (await userManager.GetRolesAsync(admin)).ToString();

            if (adminRole == UserRole.CollegeAdmin.ToString())
            {
                if (departmentDto.CollegeId != admin.CollegeId)
                { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action.")); }
            }

            var departmentExists = await dbContext.Departments.FirstOrDefaultAsync(D => (D.Name == departmentDto.Name || D.Abbreviation == departmentDto.Abbreviation) && D.CollegeId == departmentDto.CollegeId);

            if (departmentExists is not null)
                return BadRequest(new APIErrorResponse(400, "A department with the same name or Same Abbreviation and college already exists."));

            var Department = mapper.Map<Department>(departmentDto);

            await unitOfWork.Repository<Department>().AddAsync(Department);
            await unitOfWork.CompleteAsync();

            return Ok("Department Added Succesfully");
        }

        [HttpPost("EditDepartment/{Id}")]
        public async Task<ActionResult> EditDepartment(int Id, DepartmentDto departmentDto)
        {
            // Check if Department with Id Exists
            var Department = await unitOfWork.Repository<Department>().GetByIdAsync<int>(Id);
            if (Department == null) return NotFound(new APIErrorResponse(404,"Department Not Found."));

            // Check if Department with the same NAME & College Exists
            var departmentExists = await dbContext.Departments.FirstOrDefaultAsync(D => (D.Name == departmentDto.Name || D.Abbreviation == departmentDto.Abbreviation) && D.CollegeId == departmentDto.CollegeId);

            if (departmentExists is not null)
                return BadRequest(new APIErrorResponse(400, "A department with the same name or Same Abbreviation and college already exists."));


            var adminEmail = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var adminRole = (await userManager.GetRolesAsync(admin)).ToString();

            if (adminRole == UserRole.CollegeAdmin.ToString())
            {
                if (Department.CollegeId != admin.CollegeId)
                { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action.")); }
            }


            Department = mapper.Map<Department>(departmentDto);
            unitOfWork.Repository<Department>().Update(Department);
            await unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpGet("GetById/{Id}")]
        public async Task<ActionResult<DepartmentWithUserDto>> GetDepartmentById(int Id)
        {
            // Check if Department with Id Exists
            var Department = await unitOfWork.Repository<Department>().GetByIdAsync<int>(Id);
            if (Department == null) return NotFound(new APIErrorResponse(404, "Department Not Found."));

            var adminEmail = User.FindFirstValue(ClaimTypes.Email);
            var admin = await userManager.FindByEmailAsync(adminEmail);
            var adminRole = (await userManager.GetRolesAsync(admin)).ToString();

            if (adminRole == UserRole.CollegeAdmin.ToString())
            {
                if (Department.CollegeId != admin.CollegeId)
                { return Unauthorized(new APIErrorResponse(401, "You aren't authorized to perform this action.")); }
            }

            var DepartmentDto = mapper.Map<DepartmentWithUserDto>(Department);

            return Ok(DepartmentDto);
        }

    }
}
