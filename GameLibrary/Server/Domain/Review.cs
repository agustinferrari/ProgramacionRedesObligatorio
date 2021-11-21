using Server.Utils.CustomExceptions;


namespace Server.Domain
{
    public class Review
    {
        private int _rating;
        public int Rating
        {
            get => _rating;
            set
            {
                ValidateRating(value);
                _rating = value;
            }
        }
        public string Comment { get; set; }
        public User User { get; set; }

        private void ValidateRating(int rating)
        {
            if (rating <= 0 || rating > 10)
                throw new InvalidReviewRatingException();
        }

        public override string ToString()
        {
            string result = "";
            result += "\t" + "Usuario: " + User.Name + "\n";
            result += "\t" + "Rating: " + Rating + "\n";
            result += "\t" + "Comentario: " + Comment + "\n";
            result += "================================================";
            return result;
        }
    }
}
