using EmailingSystem.Core.Contracts.Specifications.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Specification.Contract
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification() { }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>>? Criteria { get; set; } = null;
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object?>>> Includes { get; set; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object?>>>();
        public Expression<Func<T, object>>? OrderBy { get; set; } = null;
        public Expression<Func<T, object>>? OrderByDesc { get; set; } = null;

        public int Skip { get; set; } 
        public int Take { get; set; }
        public bool IsPaginated { get; set; } = false;

        public void ApplyPagination(int Skip ,int Take)
        {
            this.Skip = Skip;
            this.Take = Take;
        }
        public void AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object?>> includeExpression)
        {
            Includes.Add(includeExpression);
        }


    }

}