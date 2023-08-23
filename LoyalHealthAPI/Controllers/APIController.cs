using Microsoft.AspNetCore.Mvc;

namespace LoyalHealthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {

        [HttpGet("generate")]
        public void GenerateReview() {
            // logic for returning a review
        }
       
    }
}
