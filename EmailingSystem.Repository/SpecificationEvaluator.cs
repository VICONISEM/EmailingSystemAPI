using EmailingSystem.Core.Contracts.Specifications.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace EmailingSystem.Repository
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> startQuery, ISpecification<T> spec)
        {
            var query = startQuery.AsQueryable();

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if (!spec.Includes.IsNullOrEmpty())
            {
                query = spec.Includes.Aggregate(query, (currentQuery, include) => include(currentQuery));
            }

            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsPaginated)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}