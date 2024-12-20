using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Entities;
using EmailingSystem.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmailDbContext DbContext;
        private Hashtable Repositories;

        public UnitOfWork(EmailDbContext DbContext)
        {
            Repositories = new Hashtable();
            this.DbContext = DbContext;
        }
        
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await DbContext.Database.BeginTransactionAsync();
        }
        public async Task<int> CompleteAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await DbContext.DisposeAsync();
        }
        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!Repositories.ContainsKey(type))
            {
                IGenericRepository<T> Repo = new GenericRepository<T>(DbContext);
                Repositories.Add(type, Repo);
            }

            return (IGenericRepository<T>)Repositories[type];
        }
    }
}
