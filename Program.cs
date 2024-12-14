using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourismAPI.Data;
using TourismAPI.Models;
using TourismAPI.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace TourismAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<TourismContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<TourismContext>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true; // Enforce unique email addresses
            });

           // make cookie session for 30 days 
           builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
            {
               // Configure cookie expiration to 30 days for "Remember Me"
               options.ExpireTimeSpan = TimeSpan.FromDays(30);

               // resetting  expiration on each request if user is active
               options.SlidingExpiration = true;

              
           });
            // Cors Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy => {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                });


            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Roles making Service 
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleSeeder.SeedRoles(services);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseCors("MyPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}
