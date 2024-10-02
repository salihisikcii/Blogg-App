using BlogApps.Entity;
using BlogApps.Data.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFPostRepository : IPostRepository
    {
        private BlogContext _context;

        public EFPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;
                _context.SaveChanges();
            }

        }

        public void EditPost(Post post, int[] TagIds)
        {
            var entity = _context.Posts.Include(i=>i.tags).FirstOrDefault(i => i.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;
                entity.tags= _context.Tags.Where(tag=> TagIds.Contains(tag.TagId)).ToList();
                _context.SaveChanges();
            }
        }
    }
}
