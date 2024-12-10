using EmailingSystem.Core.Contracts.Repository.Contracts;
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

        public IQueryable<T> GetAllAsync()
        {
            return _dbContext.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public async Task AddAsync(T Entity)
        {
            await _dbContext.Set<T>().AddAsync(Entity);
        }

        public void UpdateAsync(T Entity)
        {
            _dbContext.Set<T>().Update(Entity);
        }

        public void DeleteAsync(T Entity)
        {
            _dbContext.Set<T>().Remove(Entity);
        }
    }
}
