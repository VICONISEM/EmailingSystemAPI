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
            (string.IsNullOrEmpty(Specs.Search) ||
            U.NormalizedEmail.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase) ||
            (U.NormalizedName != null && U.NormalizedName.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)) ||
            (U.College != null && U.College.Name.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)) ||
            (U.Department != null && U.Department.Name.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)) ||
            (U.College != null && U.College.Abbreviation.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)) ||
            (U.Department != null && U.Department.Abbreviation.Contains(Specs.Search, StringComparison.OrdinalIgnoreCase)));

        }
    }
}
