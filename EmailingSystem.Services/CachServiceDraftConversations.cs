using EmailingSystem.Core.Contracts;
using EmailingSystem.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailingSystem.Services
{
    public class CachServiceDraftConversations (IMemoryCache cache):ICachService<DraftConversations>
    {
        private readonly IMemoryCache _memoryCache = cache;
        public async Task DeleteWithKey(string Key)
        {
            _memoryCache.Remove(Key);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<DraftConversations>?> GetValue(string Key)
        {
            if (_memoryCache.TryGetValue(Key, out IEnumerable<DraftConversations>? result))
            {
                return await Task.FromResult(result);
            }
            else
            { 
            }
            return await Task.FromResult(new List<DraftConversations>());
        }

        public async Task SetValue(string key, DraftConversations value)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(30))
                .SetPriority(CacheItemPriority.NeverRemove);
            _memoryCache.Set(key, value, options);

        }
    }
}
