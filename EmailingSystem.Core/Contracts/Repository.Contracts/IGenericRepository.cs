﻿using EmailingSystem.Core.Contracts.Specifications.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Repository.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAllQueryableWithSpecs(ISpecification<T> Specs);
        public Task<T?> GetByIdAsync(int Id);
        public Task AddAsync(T Entity);
        public void UpdateAsync(T Entity);
        public void DeleteAsync(T Entity);

    }
}
