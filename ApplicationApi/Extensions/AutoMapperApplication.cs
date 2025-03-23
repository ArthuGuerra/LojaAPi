
using ApplicationApi.DataTransferObject;
using AutoMapper;
using Domain.Entities;

namespace Domain.Otimizacao
{
    public class AutoMapperApplication : Profile
    {
        public AutoMapperApplication() 
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
