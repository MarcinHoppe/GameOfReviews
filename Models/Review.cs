using System;

namespace Reviews.Models
{
    public class Review
    {
        public Review(string comment, string author)
        {
            Comment = comment;
            Author = author;
        }

        public string Id = Guid.NewGuid().ToString();
        public string Comment { get; }
        public string Author { get; }
    }
}