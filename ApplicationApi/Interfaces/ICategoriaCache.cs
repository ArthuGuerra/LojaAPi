using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using Domain.Entities;

namespace ApplicationApi.Interfaces
{
    public interface ICategoriaCache : ICacheService<Categoria>
    {
    }
}
