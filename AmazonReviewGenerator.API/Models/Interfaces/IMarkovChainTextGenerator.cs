using System.Collections.Generic;

namespace AmazonReviewGenerator.API.Models.Interfaces
{
    public interface IMarkovChainTextGenerator
    {
        string GenerateMarkovString();

    }
}
