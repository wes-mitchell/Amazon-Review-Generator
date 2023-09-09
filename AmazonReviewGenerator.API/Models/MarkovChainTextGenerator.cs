using AmazonReviewGenerator.API.Models.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace AmazonReviewGenerator.API.Models
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

        private static string createKeyFromPrefixWithSuffix(string prefix, string suffix)
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

        public static Dictionary<string, List<string>> LoadTrainingData(int keySize, string filePath)
        {
            var reviews = new List<string>();
            using (var fileStream = File.OpenRead(filePath))
            {
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    using (var streamReader = new StreamReader(gzipStream))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                JObject reviewData = JObject.Parse(line);
                                if (reviewData.ContainsKey("reviewText"))
                                {
                                    reviews.Add(reviewData["reviewText"].ToString());
                                }
                            }
                        }
                    }
                }
            }

            var allReviewText = string.Join(" ", reviews).Split();

            Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < allReviewText.Length - keySize; i++)
            {
                var key = allReviewText.Skip(i).Take(keySize).Aggregate(createKeyFromPrefixWithSuffix);
                string value;
                if (i + keySize < allReviewText.Length)
                {
                    value = allReviewText[i + keySize];
                }
                else
                {
                    value = "";
                }

                if (dataDictionary.ContainsKey(key))
                {
                    dataDictionary[key].Add(value);
                }
                else
                {
                    dataDictionary.Add(key, new List<string>() { value });
                }
            }
            return dataDictionary;

        }
    }
}