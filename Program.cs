
using ImgriffStorage.Services;
using ImgriffStorage.Services.AzureBlobStorageServices;
using Microsoft.OpenApi.Models;
using Minio;

namespace ImgriffStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

            builder.Services.AddControllers();
            builder.Services.RegistrateServices();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Upload API", Version = "v1" });
            });

            var app = builder.Build();
            // Test

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
