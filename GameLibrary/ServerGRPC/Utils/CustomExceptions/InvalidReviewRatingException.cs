using System;

namespace ServerGRPC.Utils.CustomExceptions
{
    public class InvalidReviewRatingException : Exception
    {
        public override string Message => "Rating no se encuentra dentro del rango 1-10";
    }
}
