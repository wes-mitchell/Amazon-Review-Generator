using System.Collections.Generic;

namespace AmazonReviewGenerator.API.Models.Interfaces
{
    public interface IMarkovChainTextGenerator
    {
        string GenerateMarkovString();

        Dictionary<string, List<string>> LoadTrainingData(int keySize, string filePath);
    }
}
