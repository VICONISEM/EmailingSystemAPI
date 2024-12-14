using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specifications.Contracts
{
    public interface ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }
        public Expression<Func<T,object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
