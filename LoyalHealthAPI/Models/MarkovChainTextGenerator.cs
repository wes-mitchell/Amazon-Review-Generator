using LoyalHealthAPI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoyalHealthAPI.Models
{
    public class MarkovChainTextGenerator : IMarkovChainTextGenerator
    {
        private readonly Dictionary<string, List<string>> _reviewData;
        private readonly int _keySize;
        private readonly int _reviewLength;

        public MarkovChainTextGenerator(TrainingData trainingData)
        {
            _reviewData = trainingData.ReviewData;
            _keySize = trainingData.KeySize;
            _reviewLength = trainingData.OutputSize;
        }

        private string createKeyFromPrefixWithSuffix(string prefix, string suffix)
        {
            return $"{prefix} {suffix}";
        }

        public string GenerateMarkovString()
        {
            Random randomGenerator = new Random();
            List<string> output = new List<string>();
            int currentWordIndex = 0;
            int randomIndex = randomGenerator.Next(_reviewData.Count);
            string currentPrefix = _reviewData.Keys.Skip(randomIndex).Take(1).Single();
            output.AddRange(currentPrefix.Split());

            while (true)
            {
                var suffix = _reviewData[currentPrefix];
                if (suffix.Count == 1)
                {
                    if (suffix[0] == "")
                    {
                        return output.Aggregate(createKeyFromPrefixWithSuffix);
                    }
                    output.Add(suffix[0]);
                }
                else
                {
                    randomIndex = randomGenerator.Next(suffix.Count);
                    output.Add(suffix[randomIndex]);
                }
                if (output.Count >= _reviewLength)
                {
                    return output.Take(_reviewLength).Aggregate(createKeyFromPrefixWithSuffix);
                }
                currentWordIndex++;
                currentPrefix = output.Skip(currentWordIndex).Take(_keySize).Aggregate(createKeyFromPrefixWithSuffix);
            }
        }
    }
}