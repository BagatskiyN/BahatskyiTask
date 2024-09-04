
using HelsiTestTask.BL.Interfaces;
using HelsiTestTask.BL.Services;
using HelsiTestTask.DAL.Interfaces;
using HelsiTestTask.DAL.Repositories;
using HelsiTestTask.WebApi.Infrastructure;
using MongoDB.Driver;

namespace HelsiTestTask.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var settings = builder.Configuration.GetSection("MongoDBSettings");
                return new MongoClient(settings["ConnectionString"]);
            });

            builder.Services.AddScoped(sp =>
            {
                var settings = builder.Configuration.GetSection("MongoDBSettings");
                return sp.GetRequiredService<IMongoClient>().GetDatabase(settings["DatabaseName"]);
            });
                

            builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();

            builder.Services.AddScoped<ITaskListService, TaskListService>();

            builder.Services.AddScoped<ITaskListSharingService, TaskListSharingService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
