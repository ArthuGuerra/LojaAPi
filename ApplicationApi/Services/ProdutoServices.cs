using System;
using System.Runtime.Intrinsics.X86;
using ApplicationApi.DataTransferObject;
using ApplicationApi.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Otimizacao;
using Infraestrutura.Context;
using Infraestrutura.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationApi.Services
{
    public class ProdutoServices
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        //private readonly IProdutoCache _cache;
        private readonly IMemoryCache _cache;
        private readonly IProdutoCache _cacheProd;

        private const string CacheKey = "CacheProduto";


        public ProdutoServices(IUnitOfWork uof, IMapper mapper, IMemoryCache cache, IProdutoCache cacheProd)
        {
            _uof = uof;
            _mapper = mapper;
            _cache = cache;
            _cacheProd = cacheProd;
        }

        public ProdutoServices(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
            
        }

        public async Task<IEnumerable<ProdutoDTO>> GetProdutos()
        {
           var aux = await _uof.ProdutoRepo.GetAll();

            var auxDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(aux);
            return auxDTO;
            
        }


        public async Task<ProdutoDTO> GetProdutoID(Guid id)
        {
            try
            {
                var aux = await _uof.ProdutoRepo.GetById(p => p.Id == id);

                if (aux is null)
                {
                    throw new Exception($"Produto com Id: {id} nao encontrado");
                } 
                else
                {
                    var auxDTO = _mapper.Map<ProdutoDTO> (aux);
                    return auxDTO;
                }


            }catch (Exception ex)
            {
                throw new Exception("Ocorreu uma exceção. Erro ao Procurar o Id de Produto " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }


        public async Task<ProdutoDTO> CreateProduto(ProdutoDTO dto)
        {
            try
            {
                if(dto is null)
                {
                    throw new Exception("Objeto nao pode ser nulo");
                }
                else
                {               
                    var auxProduto = _mapper.Map<Produto>(dto);

                    _uof.ProdutoRepo.Create(auxProduto);
                    await _uof.CommitAsync();

                    var auxDTO = _mapper.Map<ProdutoDTO>(auxProduto);
                    return auxDTO;
                }
                

            } catch(Exception ex)
            {
                throw new Exception("Ocorreu uma exceção. Erro ao criar um Produto" + (ex.InnerException != null ?ex.InnerException.Message : ex.Message));
            }
        }


        public async Task<ProdutoDTO> UpdateProduto(Guid id, ProdutoDTO dto)
        {
            try
            {
                //var aux = _mapper.Map<Produto>(dto);

                var aux = await _uof.ProdutoRepo.GetById(dd => dd.Id == id);

                if (aux is null)
                {
                    throw new Exception($"Produto com Id {id} nao encontrado");
                }
                else
                {

                    var prod = new ProdutoDTO
                    {
                        Nome = dto.Nome,
                        Estoque = dto.Estoque,
                        Preco = dto.Preco,
                        CategoriaId = dto.CategoriaId,
                    };

                    var auxPRODUTO = _mapper.Map<Produto>(dto);

                    _uof.ProdutoRepo.Update(id,auxPRODUTO);
                    await _uof.CommitAsync();

                    var auxDTO = _mapper.Map<ProdutoDTO>(auxPRODUTO);
                    return auxDTO;
                }
              

            } catch (Exception ex)
            {
                throw new Exception("Ocorreu uma exceção ao Tentar atualizar um Produto. " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }


        public async Task<ProdutoDTO> DeleteProduto(Guid id)
        {
            try
            {
                var aux = _uof.ProdutoRepo.Delete(id);
                await _uof.CommitAsync();

                var auxDTO = _mapper.Map<ProdutoDTO>(aux); 

                return auxDTO;


            } catch(Exception ex)
            {
                throw new Exception("Ocorreu uma exceção ao Deletar o Produto. " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }


        //public async Task<IEnumerable<ProdutoDTO>> GetAllCategoria(Guid id,ProdutoParameters prod)
        //{
        //    var pro = _uof.ProdutoRepo.GetProdutosPorCategoriaAsync(id, prod);


        //    var metadata = new
        //    {
        //        pro.Status,
        //        prod.PageSize,
        //        prod
        //    };
        //}
     
      
    }
}
