using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.CollegeSpecs
{
    public class CollegeSpecificationsGetAllCount:BaseSpecification<College>
    {
        public CollegeSpecificationsGetAllCount(CollegeSpecsParams Specs) 
        {
            Criteria = C => (string.IsNullOrEmpty(Specs.Search) || (C.Name.Trim().ToUpper() == Specs.Search || C.Abbreviation.Trim().ToUpper() == Specs.Search));
           
        }
    }
}
