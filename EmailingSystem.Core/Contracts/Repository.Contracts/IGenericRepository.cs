﻿using EmailingSystem.Core.Contracts.Specifications.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts.Repository.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAllQueryableWithSpecs(ISpecification<T> Specs);
        public Task<int> GetCountWithSpecs(ISpecification<T> Specs);
        public Task<T?> GetByIdAsync<TId>(TId? Id) where TId : struct;
        public Task AddAsync(T Entity);
        public void Update(T Entity);

        public Task UpdateRange(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expression);
        public void Delete(T Entity);

    }
}
