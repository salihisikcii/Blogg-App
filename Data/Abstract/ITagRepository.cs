using BlogApps.Entity;

namespace BlogApps.Data.Abstract
{
    public interface ITagRepository
    {
        IQueryable<Tag> Tags { get; }

        void CreateTag (Tag tag);
    }
}
