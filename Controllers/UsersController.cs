using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApps.Data.Abstract;
using BlogApps.Entity;
using BlogApps.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _usersRepository;

        public UsersController(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewvModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _usersRepository.Users.FirstOrDefault(x => x.UserName == model.UserName);
                var user2 = _usersRepository.Users.FirstOrDefault(x => x.Email == model.Email);
                if (user != null && user2 != null)
                {
                    ModelState.AddModelError("", "KUllanıcı adı ve mail adresi zaten var.");

                }
                else if (user != null)
                {
                    ModelState.AddModelError("", "KUllanıcı adı zaten var.");
                }
                else if (user2 != null)
                {
                    ModelState.AddModelError("", "Mail adresi  zaten var.");

                }
                else
                {
                    _usersRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "avatar.jpg"

                    });
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewvModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _usersRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (isUser != null)
                {
                    var userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));

                    if (isUser.Email == "salihisikci@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,

                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adı veya şifre hatalı ");
                    return View(model);
                }

            }
            return View(model);

        }

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _usersRepository
                        .Users
                        .Include(x => x.posts)
                        .Include(x => x.comments)
                        .ThenInclude(x => x.post)
                        .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}