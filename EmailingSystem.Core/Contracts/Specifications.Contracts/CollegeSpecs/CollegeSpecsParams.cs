using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts.CollegeSpecs
{
    public class CollegeSpecsParams
    {
        private int pageIndex = 1;

        public int PageIndex
        {
            get => pageIndex;
            set => pageIndex = value > 0 ? value : 1;
        }

        private int pageSize = 10;

        public int PageSize
        {
            get => pageSize;
            set => pageSize= value > 0 ? value: 10;
        }

        private string? search;

        public string ?Search
        {
            set => search = value?.Trim().ToUpper();
            get => search;
        }
    }
}
