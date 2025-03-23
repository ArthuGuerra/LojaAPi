using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojinhaApi.Controllers
{
   
    [ApiController]
    //[Route("api/teste")]
    [Route("api/v{version:ApiVersion}/teste")]
    [ApiVersion("2.0")]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
        {
            return "Teste V2 - GET - api versao 2.0";
        }
    }
}
