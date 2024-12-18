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
    public class CollegeSpecificationsGetAll:BaseSpecification<College>
    {
        public CollegeSpecificationsGetAll(CollegeSpecsParams Specs) 
        {
            Criteria = C => (string.IsNullOrEmpty(Specs.Search) || (C.Name == Specs.Search || C.Abbreviation == Specs.Search));
            AddInclude(C => C.Include(cc => cc.Departments));

            ApplyPagination(Specs.PageSize * (Specs.PageIndex - 1), Specs.PageSize);
        
        }
    }
}
