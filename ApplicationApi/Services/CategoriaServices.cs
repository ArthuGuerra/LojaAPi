using ApplicationApi.DataTransferObject;
using ApplicationApi.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace ApplicationApi.Services
{
    public class CategoriaServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly ICategoriaCache _cacheCat;
        private readonly IMemoryCache _cache;

        private const string CacheKey = "CacheCategorias";

        public CategoriaServices(IUnitOfWork uof,IMapper mapper, ICategoriaCache cacheCat, IMemoryCache cache)
        {
            _uof = uof;
            _mapper = mapper;
            _cache = cache;
            _cacheCat = cacheCat;
        }

        //public async Task<IEnumerable<CategoriaDTO>> GetCategorias()
        //{
        //    var aux = await _uof.CategoriaRepo.GetAll();

        //    var auxCat = _mapper.Map<IEnumerable<CategoriaDTO>>(aux);

        //    return auxCat;
        //}


        public async Task<IEnumerable<CategoriaDTO>> GetCategorias()
        {

            if (!_cache.TryGetValue(CacheKey, out IEnumerable<Categoria>? categorias))
            {
                categorias = await _uof.CategoriaRepo.GetAll();

                if(categorias is null || !categorias.Any())
                {
                    throw new Exception("nenhuma categoria");
                }

                _cacheCat.SetCache(CacheKey, categorias);

            }          

            var auxCat = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return auxCat;
        }


        public async Task<CategoriaDTO> CreateCategoria(CategoriaDTO dto)
        {
            try
            {
                if(dto is null)
                {
                    throw new Exception("Ocorreu uma exceção. Objeto nao pode ser nulo");
                }
                else
                {
                    var auxCat = _mapper.Map<Categoria>(dto);

                    _uof.CategoriaRepo.Create(auxCat);

                    await _uof.CommitAsync();

                    _cache.Remove(CacheKey);


                    var cacheKey = $"cache_new_categoria = {auxCat.Id}";                  


                    var auxDTO = _mapper.Map<CategoriaDTO>(auxCat);

                    _cacheCat.SetCache(cacheKey, auxCat);

                    return auxDTO;               

                }
            }catch(Exception ex)
            {
                throw new Exception("Ocorreu uma exceção. Erro ao criar Categoria" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }


        //public async Task<CategoriaDTO> GetCategoriaId(Guid id)
        //{
        //    try
        //    {
        //        var aux = await _uof.CategoriaRepo.GetById(c => c.Id == id);

        //        if(aux == null)
        //        {
        //            throw new Exception($"Categoria com Id: {id} nao encontrado");
        //        }
        //        else
        //        {
        //            var auxDTO = _mapper.Map<CategoriaDTO>(aux);
        //            return auxDTO;
        //        }


        //    }catch(Exception ex)
        //    {
        //        throw new Exception("Ocorreu um erro ao encontrar o id da categoria" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
        //    }
        //}




        public async Task<CategoriaDTO> GetCategoriaId(Guid id)
        {

            var cacheCategoriaKey = $"Id_Categoria = {id}";

            try
            {
                if(!_cache.TryGetValue(cacheCategoriaKey, out Categoria categoria ))
                {
                    categoria = await _uof.CategoriaRepo.GetById(c => c.Id == id);

                    if (categoria is not null)
                    {                       

                        _cacheCat.SetCache(cacheCategoriaKey, categoria);
                    }                                                             
                }

                var auxDTO = _mapper.Map<CategoriaDTO>(categoria);
                return auxDTO;

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao encontrar o id da categoria" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }




        public async Task<CategoriaDTO> UpdateCategoria(Guid id,  CategoriaDTO categoriaDTO)
        {
            try
            {
                var aux = _mapper.Map<Categoria>(categoriaDTO);

                if (aux.Id != id)
                {
                    throw new Exception($"Categoria com Id {id} nao encontrado");
                }
                else
                {
                    _uof.CategoriaRepo.Update(id, aux);
                    await _uof.CommitAsync();


                    var cacheKey = $"cache_cat_update = {id}";
                 
                    var catDTO = _mapper.Map<CategoriaDTO>(aux);

                    _cacheCat.SetCache(cacheKey, aux);

                    _cache.Remove(CacheKey);

                    return catDTO;
                }

            }catch(Exception ex)
            {
                throw new Exception("");
            }
        }


        public async Task<CategoriaDTO> DeleteCategoria(Guid id)
        {
            try
            {
                var aux = _uof.CategoriaRepo.Delete(id);
                await _uof.CommitAsync();


                _cache.Remove($"cachecategoria{id}");
                _cache.Remove(CacheKey);             

                var auxDTO = _mapper.Map<CategoriaDTO>(aux);
                        
                return auxDTO;


            } catch(Exception ex)
            {
                throw new Exception("Ocorreu um erro ao Deletar uma  Categoria" + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }

        ////public async Task<IQueryable<CategoriaDTO>> GetCategoriasInProduto()
        ////{
        ////    var obj = _uof.CategoriaRepo.Include(p => p.Produto).ToList();
        ////}
        ///     
    }
}
