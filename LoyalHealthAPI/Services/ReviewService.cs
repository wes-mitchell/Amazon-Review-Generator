using AmazonReviewGenerator.API.Models;
using AmazonReviewGenerator.API.Models.Interfaces;
using AmazonReviewGenerator.API.Services.Interfaces;
using System;

namespace AmazonReviewGenerator.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMarkovChainTextGenerator _markovChainTextGenerator;

        public ReviewService(IMarkovChainTextGenerator markovChainTextGenerator) {
            _markovChainTextGenerator = markovChainTextGenerator;
        }

        public Review GenerateReview()
        {
            var reviewText = _markovChainTextGenerator.GenerateMarkovString();
            if (String.IsNullOrWhiteSpace(reviewText)) {
                throw new Exception("An error occured while generating your review.");
            }
            var starRating = new Random().Next(1, 6);
            return new Review(starRating, reviewText);
        }

    }
}
