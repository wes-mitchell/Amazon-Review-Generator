using LoyalHealthAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoyalHealthAPI.Controllers
{
    [ApiController]
    [Route("API")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("generate")]
        public IActionResult GenerateReview()
        {
            var review = _reviewService.GenerateReview();
            return Ok(review);
        }

    }
}

