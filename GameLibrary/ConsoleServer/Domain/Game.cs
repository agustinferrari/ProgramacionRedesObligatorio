using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Domain
{
    public class Game
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Genre { get; set; }
        public string Synopsis { get; set; }
        public string PathToPhoto { get; set; }
        public List<Review> Reviews { get; set; }

        public void AddReview(Review newReview)
        {
            if (Reviews == null)
                Reviews = new List<Review>();
            Reviews.Add(newReview);
            calculateRating();
            Console.WriteLine(Rating);
        }

        private void calculateRating()
        {
            int totalSum = 0;
            foreach (Review review in Reviews)
            {
                totalSum += review.Rating;
            }
            Rating = totalSum / Reviews.Count;
        }
    }
}
