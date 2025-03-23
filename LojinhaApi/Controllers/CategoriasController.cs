using Microsoft.AspNetCore.Mvc;
using ApplicationApi.Services;
using ApplicationApi.DataTransferObject;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.RateLimiting;

namespace LojinhaApi.Controllers
{
    //[EnableCors("Politica1")]
    [ApiController]
    [Route("api/[controller]")]
    //[EnableRateLimiting("fixed")]
    [Produces("application/json")]
    public class CategoriasController : ControllerBase
    {

        //private readonly IMapper _mapper;
        private readonly CategoriaServices _services;


        public CategoriasController(CategoriaServices services)
        {
            _services = services;
        }


        //[Authorize]
        [HttpGet("AllCategorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllCategorias()
        {
            var cat = await _services.GetCategorias();

            return Ok(cat);
        }


        //[DisableCors]
        //[DisableRateLimiting]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoriaByid(Guid id)
        {
            var cat = await _services.GetCategoriaId(id);
            return Ok(cat);
        }

        [HttpPost("CreateCategoria")]
        public async Task<ActionResult<CategoriaDTO>> CreateCategoria(CategoriaDTO dto)
        {
            var cat = await _services.CreateCategoria(dto);

            return new CreatedAtRouteResult("Categorias",cat);
        }

        [HttpPut("UpdateCategoria")]
        public async Task<ActionResult<CategoriaDTO>> UpdateCategoria(Guid id, CategoriaDTO dto)
        {
            var cat = await _services.UpdateCategoria(id, dto);

            return Ok(cat);
        }

        
        [HttpDelete("DeleteCategoria")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoriaDTO>> DeleteCategoria(Guid id)
        {
            var cat = await _services.DeleteCategoria(id);
            return Ok(cat);
        }      
    }
}
