using EmailingSystem.Core.Contracts.Specification.Contract;
using EmailingSystem.Core.Entities;
using EmailingSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.UserSpecs
{
    public class UserSpecificationsForCountPagination : BaseSpecification<ApplicationUser>
    {
        public UserSpecificationsForCountPagination(UserSpecsParams Specs, ApplicationUser Admin ,string role)
        {
            Criteria = U =>
            (U.Id != Admin.Id &&
            (role == UserRole.Admin.ToString()) ||
            (U.CollegeId == Admin.CollegeId))
            &&
            (string.IsNullOrEmpty(Specs.Search)) ||
            ((U.NormalizedEmail.Contains(Specs.Search)) ||
            (U.NormalizedName != null && U.NormalizedName.Contains(Specs.Search)) ||
            (U.College != null && U.College.Name.ToUpper().Contains(Specs.Search)) ||
            (U.Department != null && U.Department.Name.ToUpper().Contains(Specs.Search) ||
            (U.College != null && U.College.Abbreviation.ToUpper().Contains(Specs.Search)) ||
            (U.Department != null && U.Department.Abbreviation.ToUpper().Contains(Specs.Search))));

        }
    }
}
