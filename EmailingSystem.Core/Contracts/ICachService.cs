using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Core.Contracts
{
    public interface ICachService<T>
    {
        Task SetValue(string key, T value);
        Task<IEnumerable<T>> GetValue(string Key);
        Task DeleteWithKey(string Key);
       


    }
}
