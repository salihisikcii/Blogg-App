using BlogApps.Data.Concrete.EfCore;
using BlogApps.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        builder.Services.AddDbContext<BlogContext>(options =>
        {
            var config = builder.Configuration;
            var connectionsString = config.GetConnectionString("mysql_conneciton");
            //options.UseSqlite(connectionsString);

            var version = new MySqlServerVersion(new Version(8, 0, 39));

            options.UseMySql(connectionsString, version);
        });

        builder.Services.AddScoped<IPostRepository, EFPostRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<ITagRepository, EFTagRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<ICommentRepository, EFCommentRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddScoped<IUserRepository, EFUserRepository>(); // sanal verrsiyonunu veriyoruz gerçek versiyonunundan bir nesne üretip gönderecek yani efpostrepositoryden göndereecek
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/users/login";
        });


        // Configure the HTTP request pipeline.
        var app = builder.Build();
        SeedData.TestVerileriniDoldur(app);

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllerRoute(
            name: "post_by_tags",
            pattern: "posts/tag/{url}",
            defaults: new { controller = "Posts", action = "Index" }
            );
        app.MapControllerRoute(
                  name: "post_details",
                  pattern: "posts/details/{url}",
                  defaults: new { controller = "Posts", action = "Details" }
                  );
        app.MapControllerRoute(
                    name: "user_profile",
                    pattern: "profile/{username}",
                    defaults: new { controller = "Users", action = "Profile" }
        );
        app.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Posts}/{action=Index}/{id?}");
        app.Run();
    }
}