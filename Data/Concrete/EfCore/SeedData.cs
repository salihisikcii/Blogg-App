using BlogApps.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApps.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = " web programlama", Url = "web-programlama", Color = TagColors.danger },
                        new Tag { Text = " Backend", Url = "backend", Color = TagColors.warning },
                        new Tag { Text = " Frontend", Url = "frontend", Color = TagColors.secondary },
                        new Tag { Text = " FullStack", Url = "fullstack", Color = TagColors.success },
                        new Tag { Text = " Php", Url = "php", Color = TagColors.primary }
                        );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserId = 1, UserName = "Ali16", Name = "Ali Şaşmaz", Email = "alisasmaz@gmail.com", Password = "123456", Image = "p1.jpg" },
                        new User { UserId = 2, UserName = "Salih42", Name = "Salih Işıkcı", Email = "salihisikci@gmail.com", Password = "123456", Image = "p2.jpg" }
                        );
                    context.SaveChanges();

                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                           new Post
                           {
                               Title = "Asp.net core",
                               Content = "Asp.net core dersleri",
                               Description = " Sıfırdan İleri Seviye Asp.net core dersleri",
                               Url = "aspnet-core",
                               IsActive = true,
                               PublishedOn = DateTime.Now.AddDays(-10),
                               tags = context.Tags.Take(3).ToList(),
                               Image = "1.jpg",
                               UserId = 1,
                               comments = new List<Comment> {
                                new Comment { Text = "iyi bir kurs", PublishedOn = DateTime.Now.AddDays(-25), UserId = 1},
                                new Comment { Text = "çok faydalandığım bir kurs", PublishedOn =DateTime.Now.AddDays(-12), UserId = 2},
                            }
                           },
                         new Post
                         {
                             Title = "Php",
                             Content = " Php dersleri",
                             Description = " Sıfırdan İleri Seviye Php dersleri",

                             Url = "php",
                             IsActive = true,
                             PublishedOn = DateTime.Now.AddDays(-2),
                             tags = context.Tags.Take(2).ToList(),
                             Image = "2.jpg",
                             UserId = 1
                         },
                          new Post
                          {
                              Title = "Django",
                              Content = " Django dersleri",
                              Description = " Sıfırdan İleri Seviye Django dersleri",

                              Url = "django",
                              IsActive = true,
                              PublishedOn = DateTime.Now.AddDays(-5),
                              tags = context.Tags.Take(4).ToList(),
                              Image = "3.jpg",
                              UserId = 2
                          },
                             new Post
                             {
                                 Title = "React Dersleri",
                                 Content = "React Dersleri",
                                 Description = " Sıfırdan İleri Seviye React dersleri",

                                 Url = "react-dersleri",
                                 IsActive = true,
                                 PublishedOn = DateTime.Now.AddDays(-26),
                                 tags = context.Tags.Take(4).ToList(),
                                 Image = "4.jpg",
                                 UserId = 2
                             },
                             new Post
                             {
                                 Title = "Angular",
                                 Content = " Angular dersleri",
                                 Description = " Sıfırdan İleri Seviye Angular dersleri",

                                 Url = "angular",
                                 IsActive = true,
                                 PublishedOn = DateTime.Now.AddDays(-55),
                                 tags = context.Tags.Take(4).ToList(),
                                 Image = "5.jpg",
                                 UserId = 2
                             },
                             new Post
                             {
                                 Title = "Web Tasarım",
                                 Content = " Web tasarım Dersleri",
                                 Description = " Sıfırdan İleri Seviye Angular dersleri",
                                 Url = "web-tasarim",
                                 IsActive = true,
                                 PublishedOn = DateTime.Now.AddDays(-27),
                                 tags = context.Tags.Take(4).ToList(),
                                 Image = "6.jpg",
                                 UserId = 2
                             }

                        );
                    context.SaveChanges();

                }
            }
        }
    }
}
