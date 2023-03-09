
using Microsoft.EntityFrameworkCore;
using WebAPIDay1.Models;

namespace WebAPIDay1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string text = "allow";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApiDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("connection")));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(text,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}