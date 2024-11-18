
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourismAPI.Data;
using TourismAPI.Models;

namespace TourismAPI
{
    public class Program
    {
        public static void Main(string[] args)
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
