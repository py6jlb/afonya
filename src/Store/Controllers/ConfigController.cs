using ExternalConfig.Models;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public IActionResult Get(string appName)
        {
            var section = _configuration.GetSection(appName);
            if (!section.Exists()) return BadRequest();

            var dict = section.GetChildren().ToDictionary(x => x.Key, x => x.Value);

            return Ok( new ConfigResult
            {
                Code = 0,
                Data = dict,
                Msg = "OK"
            });
        }
    }
}
