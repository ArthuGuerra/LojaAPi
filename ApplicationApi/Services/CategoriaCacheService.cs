using ApplicationApi.DataTransferObject;
using ApplicationApi.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infraestrutura.Context;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.Services
{
    public class CategoriaCacheService : CacheService<Categoria>, ICategoriaCache
    {
        public CategoriaCacheService(IMemoryCache cache) : base(cache)
        {
        }
    }
}
