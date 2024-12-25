using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.CollegeSpecs;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs.College;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
       private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CollegeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("GetAllColleges")]
        public async Task<ActionResult<Pagination<CollegesDto>>> GetAllCollegesWithSpecsAsync([FromQuery]CollegeSpecsParams SpecsParams)
        {
            var Specs = new CollegeSpecificationsGetAll(SpecsParams);
            var colleges=await unitOfWork.Repository<College>().GetAllQueryableWithSpecs(Specs).ToListAsync();
            var Colleges = mapper.Map<List<CollegesDto>>(colleges);

            var SpecsCount = new CollegeSpecificationsGetAllCount(SpecsParams);
            var CollegesCount = await unitOfWork.Repository<College>().GetCountWithSpecs(SpecsCount);

            return Ok(new Pagination<CollegesDto>(SpecsParams.PageIndex,SpecsParams.PageSize,CollegesCount,Colleges));
        }

        [HttpPost("UpdateCollege")]
        public async Task<ActionResult>UpdateCollege([FromForm]CollegesDto college , int ?Id)
        {
            if(college is null || Id is null)
            {
                return BadRequest("There Is Error With Dto Or Id");
            }
            var College = await unitOfWork.Repository<College>().GetByIdAsync(Id);
            if(College is null)
            {
                return BadRequest("There Is Error When Fetching College");
            }
            var Specs = new CollegeSpecificationCheckCollege(college.Name,college.Abbreviation);
            var IfExist = await unitOfWork.Repository<College>().GetAllQueryableWithSpecs(Specs).FirstOrDefaultAsync();
            if(IfExist is not null)
            {
                return BadRequest(new APIErrorResponse(400, "The College Exists already"));
            }
            if(college.Name.IsNullOrEmpty() && college.Abbreviation.IsNullOrEmpty())
            {
                return BadRequest("No Data To Update");

            }

          else if(college.Name.IsNullOrEmpty())
            {
                College.Abbreviation = college.Abbreviation;
            }
           else if(college.Abbreviation.IsNullOrEmpty())
            {
                College.Name = college.Name;
            }

            else
            {
                mapper.Map(college, College);
            }

            unitOfWork.Repository<College>().Update(College);
            await unitOfWork.CompleteAsync();
            return Ok($"College Updated Successfully"); 

        
        }

        [HttpPost("AddCollege")]
        public async Task<ActionResult<CollegeAddDto>>AddCollege(CollegeAddDto college) 
        {
            var Specs = new CollegeSpecificationCheckCollege(college.Name,college.Abbreviation);
            var IfExist = await unitOfWork.Repository<College>().GetAllQueryableWithSpecs(Specs).FirstOrDefaultAsync();
            if(IfExist is null)
            {
                await unitOfWork.Repository<College>().AddAsync(mapper.Map<College>(college));
                await unitOfWork.CompleteAsync();
                return Ok("College Added Successfully");
            }
            return BadRequest(new APIErrorResponse(400,"The College Exists already"));
        }

        //[HttpPost("DeleteCollegeById")]
        //public async Task<ActionResult> DeleteCollegeById(int? Id)
        //{
        //    if (Id is null)
        //        return BadRequest("there Is No Id");

        //    var College = await unitOfWork.Repository<College>().GetByIdAsync(Id);
        //    if (College is null)
        //        return BadRequest(new { error = "there Is No College With This Id" });
        //    unitOfWork.Repository<College>().Delete(College);
        //    return Ok($"College Deleted Successfully");
        //}
    }
}
