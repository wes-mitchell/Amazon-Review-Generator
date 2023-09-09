using AmazonReviewGenerator.API.Models;

namespace AmazonReviewGenerator.API.Services.Interfaces
{
    public interface IReviewService
    {
        public Review GenerateReview();
    }
}
