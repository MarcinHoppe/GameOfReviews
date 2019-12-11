using System;

namespace Reviews.Models
{
    public class Review
    {
        public Review(string text, string author)
        {
            Text = text;
            Author = author;
        }

        public string Id = Guid.NewGuid().ToString();
        public string Text { get; }
        public string Author { get; }
    }
}