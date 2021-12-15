using Microsoft.AspNetCore.Mvc;
using RuleEngine.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace RuleEngine.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetTopFiveController : ControllerBase
    {
        private IDBConnector _dbConnector;
      
        public GetTopFiveController(IDBConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        [HttpGet]
        public async Task<string> GetTopFive()
        {
            var facts = await _dbConnector.GetTopMembers();
            return facts;
        }
    }
}
