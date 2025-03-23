using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.Interfaces
{
    public interface ICacheService<T> where T : class
    {  

        public void SetCache<T>(string key, T data);  
        
        public void InvalidateCacheAfterChange(Guid id, T? classe = null);

    }
}
