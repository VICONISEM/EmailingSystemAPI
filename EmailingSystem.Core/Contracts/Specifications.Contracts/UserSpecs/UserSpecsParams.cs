using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.UserSpecs
{
    public class UserSpecsParams
    {
        private int pageNumber = 1;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value <= 0 ? 1 : value; }
        }

        private int pageSize = 50;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 50 || value <= 0 ? 50 : value; }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.Trim().ToUpper(); }
        }
    }
}
}
