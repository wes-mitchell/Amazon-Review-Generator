using LoyalHealthAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace LoyalHealthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string fileName = "Musical_Instruments.json.gz";
        private readonly string dataFilePath;

        public APIController(IWebHostEnvironment webHostEnvironment) { 
            _webHostEnvironment = webHostEnvironment;
            dataFilePath = $"{_webHostEnvironment.ContentRootPath}/Files/{fileName}";
        }

        [HttpGet("generate")]
        public string GenerateReview()
        {
            var generator = new MarkovChainTextGenerator();
            return generator.Markov(dataFilePath, 3, 200);
        }

    }
}
