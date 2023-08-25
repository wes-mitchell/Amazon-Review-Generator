namespace LoyalHealthAPI.Models
{
    public class Review
    {
        public Review(int starRating, string reviewText)
        {
            StarRating = starRating;
            ReviewText = reviewText;
        }

        public int StarRating { get; set; }

        public string ReviewText { get; set; }
    }
}
