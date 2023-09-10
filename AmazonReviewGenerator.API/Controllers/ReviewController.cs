using AmazonReviewGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmazonReviewGenerator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("Generate")]
        public IActionResult GenerateReview()
        {
            var review = _reviewService.GenerateReview();
            return View("Review", review);
        }

    }
}

