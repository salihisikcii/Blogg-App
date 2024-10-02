using BlogApps.Data.Concrete.EfCore;
using BlogApps.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using BlogApps.Models;
using Microsoft.EntityFrameworkCore;
using BlogApps.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApps.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _Postrepostitory;

        private ICommentRepository _CommentRepository;

        private ITagRepository _TagRepository;
        public PostsController(IPostRepository Postrepostitory, ICommentRepository CommentRepository, ITagRepository TagRepository)
        {
            _Postrepostitory = Postrepostitory;
            _CommentRepository = CommentRepository;
            _TagRepository = TagRepository;
        }
        public async Task<IActionResult> Index(string? url)
        {
            var claims = User.Claims;
            var post = _Postrepostitory.Posts.Where(i => i.IsActive == true);

            if (!string.IsNullOrEmpty(url))
            {
                post = post.Where(x => x.tags.Any(y => y.Url == url));
            }
            return View(
                new PostsViewModel
                {
                    Posts = await post.ToListAsync(),
                }
            );
        }

        public async Task<IActionResult> Details(string? url)
        {
            var item = await _Postrepostitory
            .Posts
            .Include(x => x.user)
            .Include(x => x.tags) // tagleri dahil ediyoruz
            .Include(x => x.comments) // yorumları dahil ediyoruz
            .ThenInclude(x => x.user) // yorumu yapan kişinin bilgileri için cooments içindeki yani yorumu yapan kullanıcıyı alıyoruz
            .FirstOrDefaultAsync(x => x.Url == url);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId ?? ""),
            };
            _CommentRepository.CreateComment(entity);
            return Json(new
            {
                username,
                Text,
                entity.PublishedOn,
                avatar
            });

        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]

        public IActionResult Create(NewPostsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _Postrepostitory.CreatePost(
                    new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Description = model.Description,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        PublishedOn = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false,

                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);
            var posts = _Postrepostitory.Posts;
            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = _Postrepostitory.Posts.Include(x => x.tags).FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewBag.Tags = _TagRepository.Tags.ToList();
            return View(new NewPostsViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Url = post.Url,
                Content = post.Content,
                Description = post.Description,
                IsActive = post.IsActive,
                Tags = post.tags,
            });

        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(NewPostsViewModel model, int[] TagIds)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Url = model.Url,
                };
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }
                _Postrepostitory.EditPost(entityToUpdate, TagIds);
                return RedirectToAction("List");
            }
            ViewBag.Tags = _TagRepository.Tags.ToList();

            return View(model);
        }

    }
}
