using System;
using System.Collections.Generic;

namespace Reviews.Models
{
    public class Product
    {
        public Product(string name)
        {
            Name = name;
        }

        public string Id = Guid.NewGuid().ToString();

        public string Name { get; }

        public void AddReview(string comment, string author)
        {
            reviews.Add(new Review(comment, author));
        }

        public bool RemoveComment(string reviewId)
        {
            return reviews.Remove(reviews.Find(review => review.Id == reviewId));
        }

        public int ReviewCount
        {
            get { return reviews.Count; }
        }

        public IEnumerable<Review> AllReviews()
        {
            return reviews;
        }

        private List<Review> reviews = new List<Review>();
    }
}