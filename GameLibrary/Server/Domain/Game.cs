using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Domain
{
    public class Game
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Genre { get; set; }
        public string Synopsis { get; set; }
        public string PathToPhoto { get; set; }
        public List<Review> Reviews { get; set; }
        public User OwnerUser { get; set; }

        public void AddReview(Review newReview)
        {
            if (Reviews == null)
                Reviews = new List<Review>();
            Reviews.Add(newReview);
            CalculateRating();
        }

        private void CalculateRating()
        {
            int totalSum = 0;
            foreach (Review review in Reviews)
            {
                totalSum += review.Rating;
            }
            Rating = totalSum / Reviews.Count;
        }

        public override string ToString()
        {
            string result = "";
            result += "Nombre: " + Name + "\n";
            result += "Genero: " + Genre + "\n";
            result += "Sinopsis: " + Synopsis + "\n";
            result += "Rating promedio: " + Rating + "\n";
            result += "Calificaciones: \n";
            string reviewList = "";
            if (Reviews == null || Reviews.Count == 0)
                reviewList = "Aun no hay calificaciones";
            else
                foreach (Review review in Reviews)
                    reviewList += review.ToString() + "\n";
            result += reviewList;

            return result;
        }
    }
}
