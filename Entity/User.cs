﻿namespace BlogApps.Entity
{
    public class User
    {
        public int UserId  { get; set; }

        public string? UserName  { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public List<Post> posts { get; set; } = new List<Post>();


        public List<Comment> comments { get; set; } = new List<Comment>();

        public string? Image { get; set; }

    }
}
