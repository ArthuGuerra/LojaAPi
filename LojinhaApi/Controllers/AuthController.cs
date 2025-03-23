using ApplicationApi.DataTransferObjectLogin;
using ApplicationApi.Services;
using Azure;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace LojinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;


        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }



        //[HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> AllUsers()
        //{
        //    await _authServices.AllUsers();

        //    return Ok();
        //}


        
        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Policy = "Super")]
        public async Task<ActionResult> CreateRole(string roleName)
        {
           await _authServices.CreateRole(roleName);

            return Ok("Succees" + "\n" + roleName);
        }


        [Authorize(Policy = "Super")]
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<ActionResult> AddUser(string email, string roleName)
        {
            await _authServices.AddUserToRole(email, roleName);
            
           return Ok("Status: Succees" + "\n" + $"Usuario com email {email} foi adiconado a funçao {roleName} ");
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var aux = await _authServices.Login(model);

            return Ok(aux); 
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModelDTO model)
        {
            await _authServices.Register(model);

            //return Ok(new Response { Status = "Success", Message = "User created successfully" });

            return Ok(model);
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult> RefreshToken(TokenModelDTO model)
        {
            var aux = await _authServices.RefreshTokenMethod(model);

            return Ok(aux);
        }


        [HttpPost]
        [Authorize(Policy = "ExclusiveOnly")]  
        [Route("revoke/{username}")]

        public async Task<ActionResult> RevokeMethod(string username)
        {
            await _authServices.Revoke(username);

            return Ok(username);
        }
        
    }
}
