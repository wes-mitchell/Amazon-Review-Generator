using LoyalHealthAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace LoyalHealthAPI.Tests.TestHelpers
{
    public static class TestHelpers
    {
        public static TrainingData GenerateTrainingData(Dictionary<string, List<string>> reviewData, int keySize, int outputSize)
        {
            return new TrainingData
            {
                ReviewData = reviewData,
                KeySize = keySize,
                OutputSize = outputSize,
            };
        }

        public static Dictionary<string, List<string>> GenerateReviewData(string[] reviewText, int keySize)
        {
            reviewText = string.Join(" ", reviewText).Split();
            Dictionary<string, List<string>> dataDictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < reviewText.Length - keySize; i++)
            {
                var key = reviewText.Skip(i).Take(keySize).Aggregate(createKeyFromPrefixWithSuffix);
                string value;
                if (i + keySize < reviewText.Length)
                {
                    value = reviewText[i + keySize];
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

        public static string[] GetReviewText() {
            return new string[] { @"These Ninja Turtle toys are fantastic! My kids can't get enough of them. The details and colors are amazing.
I bought these Ninja Turtle toys for my nephew's birthday, and he hasn't stopped playing with them.A definite hit!
The quality of these toys is top - notch.I remember loving Ninja Turtles as a kid, and these toys do justice to the franchise.
These Ninja Turtle toys bring back so many memories.It's great to see the turtles in action again.
I'm impressed with the articulation on these figures. My son can pose them in various action-packed stances.
As a collector, I appreciate the attention to detail on these Ninja Turtle toys.They look fantastic on my display shelf.
The only downside is that some parts feel a bit flimsy.My son accidentally broke a weapon while playing.
These Ninja Turtle toys are a bit pricey, but they're worth it for the joy they bring to my children.
The packaging was damaged when the toys arrived. Amazon should improve their packaging for collectible items.
My daughter loves the Ninja Turtles as much as my son does.These toys are perfect for kids of all genders.
" };
        }

        public static TrainingData GetTrainingData(int keySize, int outputSize) {
            var reviewText = GetReviewText();
            var reviewData = GenerateReviewData(reviewText, keySize);
            var trainingData = GenerateTrainingData(reviewData, keySize, outputSize);
            return trainingData;
        }

        private static string createKeyFromPrefixWithSuffix(string prefix, string suffix)
        {
            return $"{prefix} {suffix}";
        }
    }
}
