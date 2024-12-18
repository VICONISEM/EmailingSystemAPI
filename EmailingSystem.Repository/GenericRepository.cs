using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Contracts.Specifications.Contracts;
using EmailingSystem.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EmailDbContext _dbContext;

        public GenericRepository(EmailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> GetAllQueryableWithSpecs(ISpecification<T> Specs)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Specs).AsNoTracking();
        }

        public async Task<int> GetCountWithSpecs(ISpecification<T> Specs)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Specs).CountAsync();
        }

        public async Task<T?> GetByIdAsync<TId>(TId ?Id) where TId : struct
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public async Task AddAsync(T Entity)
        {
            await _dbContext.Set<T>().AddAsync(Entity);
        }

        public void Update(T Entity)
        {
            _dbContext.Set<T>().Update(Entity);
        }

        public void Delete(T Entity)
        {
            _dbContext.Set<T>().Remove(Entity);
        }
    }
}
