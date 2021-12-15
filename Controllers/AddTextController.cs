using Microsoft.AspNetCore.Mvc;
using RuleEngine.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuleEngine.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddTextController : ControllerBase
    {
        private IDBConnector _connector;
        private static JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        public AddTextController(IDBConnector connector)
        {
            _connector = connector;
        }

       [HttpGet]
        public async Task AddText([FromQuery] string text)
        {
             await _connector.AddString(text);
        }
    }
}
