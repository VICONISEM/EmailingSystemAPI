using EmailingSystem.Core.Contracts.Specification.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository
{
    public static class SpecificationEvalutor<T> where T : class
    {

        public static IQueryable<T>GetQuery(IQueryable<T> StartQuery,ISpecification<T> spec)
        {
            var query = StartQuery.AsQueryable();

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }


            if(!spec.Includes.IsNullOrEmpty())
            {
               query= spec.Includes.Aggregate(query, (CurrentQuery, Include) => CurrentQuery.Include(Include));
            }

            return query;


        }




    }
    


    
}
