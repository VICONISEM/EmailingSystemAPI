using AutoMapper;
using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts.CollegeSpecs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
       private readonly IUnitOfWork unitOfWork;

        public CollegeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult<>> GetAllCollegesWithSpecsAsync(CollegeSpecsParams SpecsParams)
        {
            var Colloges=
        }

    }
}
