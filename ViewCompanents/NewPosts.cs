
using System.ComponentModel;
using BlogApps.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.ViewCompanents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _Postrepostitory;
        public NewPosts(IPostRepository Postrepostitory)
        {
            _Postrepostitory = Postrepostitory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View( await _Postrepostitory.
            Posts.
            OrderByDescending(p=> p.PublishedOn).
            Take(5).
            ToListAsync());
        }
    }
}