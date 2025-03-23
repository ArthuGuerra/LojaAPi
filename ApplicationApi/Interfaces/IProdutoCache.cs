using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using Domain.Entities;
using Infraestrutura.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.Interfaces
{
    public interface IProdutoCache : ICacheService<ProdutoDTO>
    {
    }
}
