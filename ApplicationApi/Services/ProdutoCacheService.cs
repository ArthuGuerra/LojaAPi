using ApplicationApi.DataTransferObject;
using ApplicationApi.Interfaces;
using Domain.Entities;
using Infraestrutura.Context;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.Services
{
    public class ProdutoCacheService : CacheService<ProdutoDTO>, IProdutoCache
    {
        public ProdutoCacheService(IMemoryCache cache) : base(cache)
        {
        }
    }
}
