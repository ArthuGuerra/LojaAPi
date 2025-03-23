using ApplicationApi.DataTransferObject;
using ApplicationApi.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojinhaApi.Controllers
{
    //[EnableCors("Politica2")]
    [Route("api/[controller]")]
    [ApiController]
    //[Produces("application/json")]
    public class ProdutosController : ControllerBase
    {
        //private readonly IMapper _mapper;
        private readonly ProdutoServices _services;

        public ProdutosController(ProdutoServices services)
        {
            _services = services;
        }

        //[Authorize(Policy = "UserOnly")]
        [HttpGet("AllProdutos")]
        public async Task<ActionResult<ProdutoDTO>> GetAllProdutos()
        {
            var prod = await _services.GetProdutos();

            //var produto = _mapper.Map<IEnumerable<ProdutoDTO>>(prod);

            return Ok(prod);
        }

        [HttpGet("Id")]
        public async Task<ActionResult<ProdutoDTO>> GetByID(Guid id)
        {

            var prod = await _services.GetProdutoID(id);

            //var produto = _mapper.Map<ProdutoDTO>(prod);

            return Ok(prod);
        }


        [HttpPost("Create")]
        public async Task<ActionResult<ProdutoDTO>> CreateProduto(ProdutoDTO dto)
        {
          
            var prod = await _services.CreateProduto(dto);


            //var prodDTO = _mapper.Map<ProdutoDTO>();

            return new CreatedAtRouteResult("NovoProd",prod);
        }

        //[Authorize]
        [HttpPut]
        public async Task<ActionResult<ProdutoDTO>> UpdateProduto(Guid id, ProdutoDTO dto)
        {
            var prod = await _services.UpdateProduto(id, dto);

            return Ok(prod);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<ActionResult<ProdutoDTO>> Delete(Guid id)
        {
            var aux = await _services.DeleteProduto(id);

            return Ok(aux);
        }      
    }
}
