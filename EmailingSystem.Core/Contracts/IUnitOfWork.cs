using EmailingSystem.Core.Contracts.Repository.Contracts;
using EmailingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IGenericRepository<T> Repository<T>() where T : class;
        public Task<int> CompleteAsync();

    }
}
