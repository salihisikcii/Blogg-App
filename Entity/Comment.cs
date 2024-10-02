namespace BlogApps.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }  

        public string? Text { get; set; }

        public DateTime PublishedOn { get; set; }

        public int PostId { get; set; }
        public Post post { get; set; } = null!;

        public int UserId { get; set; }
        public User user { get; set; } = null!;
    }
}
