using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.CollegeSpecs;
using EmailingSystem.Core.Entities;
using EmailingSystemAPI.DTOs;
using EmailingSystemAPI.Errors;
using EmailingSystemAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult>UpdateCollege(CollegesDto college , int ?Id)
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

            mapper.Map(college, College);

            unitOfWork.Repository<College>().Update(College);
            return Ok($"College Updated Successfully"); 

        }

        [HttpPost("AddCollege")]
        public async Task<ActionResult<CollegeAddDto>>AddCollege(CollegeAddDto college) 
        {
            var Specs = new CollegeSpecificationCheckCollege(college.Name);
            var IfExist = await unitOfWork.Repository<College>().GetAllQueryableWithSpecs(Specs).ToListAsync();
            if(IfExist is null)
            {
                await unitOfWork.Repository<College>().AddAsync(mapper.Map<College>(college));
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
