using Handlingform.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Handlingform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure the DbContext to use SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Session services
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
                options.Cookie.IsEssential = true; // Make the session cookie essential for the app to function
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
            });

            // Add Cookie Authentication for managing user sessions
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login"; // Redirect to login page if user is not authenticated
                    options.LogoutPath = "/User/Logout"; // Redirect to logout action
                    options.AccessDeniedPath = "/User/AccessDenied"; // Optional: Handle access denied
                    options.SlidingExpiration = true; // Keep the session alive as long as the user is active
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration time
                });

            var app = builder.Build();

            // Enable session middleware
            app.UseSession();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                IApplicationBuilder applicationBuilder = app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable authentication and authorization
            app.UseAuthentication(); // Add authentication middleware
            app.UseAuthorization();  // Add authorization middleware

            // Map the default controller route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
