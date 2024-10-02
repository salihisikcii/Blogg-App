using BlogApps.Entity;
using BlogApps.Data.Abstract;

namespace BlogApps.Data.Concrete.EfCore
{
    public class EFTagRepository : ITagRepository
    {
        private BlogContext _context;

        public EFTagRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag Tag)
        {
            _context.Tags.Add(Tag);
            _context.SaveChanges();
        }
    }
}
