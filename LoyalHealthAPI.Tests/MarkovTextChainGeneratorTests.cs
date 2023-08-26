using LoyalHealthAPI.Models;
using TestHelper = LoyalHealthAPI.Tests.TestHelpers.TestHelpers;
using Xunit;

namespace LoyalHealthAPI.Tests
{
    [Collection("Sequential")]

    public class MarkovTextChainGeneratorTests
    {
        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(10)]
        public void MarkovChainTextGenerator_GenerateMarkovString_OutputsResultsUnderLength(int outputSize)
        {
            int keySize = 2;
            int _outputSize = outputSize;
            var trainingData = TestHelper.GetTrainingData(keySize, _outputSize);
            var generator = new MarkovChainTextGenerator(trainingData);

            var result = generator.GenerateMarkovString();
            var maxExpectedOutput = outputSize;
            var actualExpectedOutput = result.Split().Length;

            Assert.True(maxExpectedOutput >= actualExpectedOutput);
        }
    }
}

