using System;

namespace ServerGRPC.Utils.CustomExceptions
{
    public class InvalidReviewRatingException : Exception
    {
        public override string Message => "The rating was not between the range 1-10";
    }
}
