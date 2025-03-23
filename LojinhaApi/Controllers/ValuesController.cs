using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojinhaApi.Controllers
{
    [Route("api/teste")]
    [ApiController]
    [ApiVersion(3)]
    [ApiVersion(4)]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion(3)]
        public string GetVersion3()
        {
            return "version3 - GET - api versao 3";
        }

        [HttpGet]
        [MapToApiVersion(4)]
        public string GetVersion4()
        {
            return "version4 - GET - api versao 4";
        }
    }
}
