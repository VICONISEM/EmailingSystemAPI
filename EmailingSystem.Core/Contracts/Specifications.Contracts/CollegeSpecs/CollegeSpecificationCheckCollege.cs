using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.CollegeSpecs
{
    public class CollegeSpecificationCheckCollege:BaseSpecification<College>
    {
        public CollegeSpecificationCheckCollege(string Name)
        {
            Criteria = Q=>Q.Name.Trim().ToUpper()==Name.Trim().ToUpper();
        }
    }
}
