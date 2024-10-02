namespace BlogApps.Entity
{
    public class Post
    {
        public int PostId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Content { get; set; }
        public string? Url { get; set; }

        public string? Image { get; set; }

        public DateTime PublishedOn { get; set; }

        public bool IsActive { get; set; }  

        public int UserId { get; set; }

        public User user { get; set; } = null!;
        public List<Tag> tags { get; set; } = new List<Tag>();

        public List<Comment> comments { get; set; } = new List<Comment>();

    }
}
