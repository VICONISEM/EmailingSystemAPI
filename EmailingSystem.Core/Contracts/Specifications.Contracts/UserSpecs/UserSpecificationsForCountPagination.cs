using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.UserSpecs
{
    public class UserSpecificationsForCountPagination : BaseSpecification<ApplicationUser>
    {
        public UserSpecificationsForCountPagination(UserSpecsParams Specs, ApplicationUser Admin)
        {
            Criteria = U =>
                (U.Id != Admin.Id &&

                (!U.CollegeId.HasValue || U.CollegeId == Admin.CollegeId))
                &&
                (string.IsNullOrEmpty(Specs.Search) || U.NormalizedName.Contains(Specs.Search));

        }
    }
}
