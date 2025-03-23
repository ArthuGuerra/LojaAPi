using ApplicationApi.DataTransferObject;
using ApplicationApi.Interfaces;
using Domain.Entities;
using Infraestrutura.Context;
using Infraestrutura.PadraoUnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.Services
{
    public class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "Produtos";

        public string GetCacheKey(Guid id) => $"Cache_{id}";

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;          
        }
     

        public void SetCache<T>(string key, T data)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(15),
                Priority = CacheItemPriority.High
            };

            _cache.Set(key, data, options);
        }

        public void InvalidateCacheAfterChange(Guid id, T? classe = null)
        {
            _cache.Remove(CacheKey);
            //_cache.Remove(GetCacheKey(id));


            if(classe != null)
            {
                SetCache(GetCacheKey(id), classe);
            }
        }
    }
}

