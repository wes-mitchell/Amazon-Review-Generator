using LoyalHealthAPI.Interfaces;
using LoyalHealthAPI.Models;
using System;

namespace LoyalHealthAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly MarkovChainTextGenerator markovChainTextGenerator;
        private readonly TrainingData _trainingData;

        public ReviewService(TrainingData trainingData) {
            _trainingData = trainingData;
            markovChainTextGenerator = new MarkovChainTextGenerator(_trainingData);
        }

        public Review GenerateReview()
        {
            var reviewText = markovChainTextGenerator.GenerateMarkovString();
            if (String.IsNullOrWhiteSpace(reviewText)) {
                throw new Exception("An error occured while generating your review.");
            }
            var starRating = new Random().Next(1, 6);
            return new Review(starRating, reviewText);
        }

    }
}
