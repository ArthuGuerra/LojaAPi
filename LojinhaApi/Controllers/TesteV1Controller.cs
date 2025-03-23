using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LojinhaApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/teste")]
    //[Route("api/teste")]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
        {
            return " Teste V1 - GET - api versao 1.0";
        }
    }
}
