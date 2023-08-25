using System.Collections.Generic;

namespace LoyalHealthAPI.Models
{
    public class TrainingData
    {
        public Dictionary<string, List<string>> ReviewData { get; set; }

        public int KeySize { get; set; }

        public int OutputSize { get; set; }

    }

}
