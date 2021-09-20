using ConsoleServer.Utils.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleServer.Domain
{
    public class Review
    {
        private int _rating;
        public int Rating
        {
            get => _rating;
            set
            {
                validateRating(value);
                _rating = value;
            }
        }
        public string Comment { get; set; }
        public User User { get; set; }

        private void validateRating(int rating)
        {
            if (rating <= 0 || rating > 10)
                throw new InvalidReviewRatingException();
        }
    }
}
