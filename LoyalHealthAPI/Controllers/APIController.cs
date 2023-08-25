using LoyalHealthAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LoyalHealthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly MarkovChainTextGenerator markovChainTextGenerator;
        private readonly TrainingData _trainingData;

        public APIController(TrainingData trainingData)
        {
            _trainingData = trainingData;
            markovChainTextGenerator = new MarkovChainTextGenerator(_trainingData);
        }

        [HttpGet("generate")]
        public IActionResult GenerateReview()
        {
            var reviewText = markovChainTextGenerator.GenerateMarkovString();
            var starRating = new Random().Next(1, 6);
            return Ok(new Review(starRating, reviewText));
        }

    }
}

