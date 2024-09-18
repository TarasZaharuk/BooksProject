using BooksProject.Shared;
using BooksWebApi.Abstractions;
using BooksWebApi.Repositories;
using DataBaseManeger;
using DataBaseManeger.Abstractions;

namespace BooksWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddTransient<IBooksRepository, InMemoryBooksRepository>();
            builder.Services.AddTransient<IDataBase<BookDetailsDto>, DBManipulator<BookDetailsDto>>();
            //builder.Services.AddTransient<IBooksRepository, FileBooksRepository>();
            builder.Services.AddTransient<IBooksRepository, SQLBooksRepository>();
            builder.Services.AddTransient<SQLBooksRepository>();
            builder.Services.AddScoped<DBManipulator>();
            var app = builder.Build();

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            var config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            config.AddEnvironmentVariables();
            config.Build();
            var settings = config.Build().GetRequiredSection("DataBasePathSettings").Get<AppDataBasePathSettings>() ?? throw new Exception("settings is null(no path)");
            DBManipulator <BookDetailsDto> dBManipulator = new();
            dBManipulator.SetPath(settings.Path);

            app.Run();
        }
    }
}
