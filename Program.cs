using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PresenceBackend.Models.Database;
using PresenceBackend.Modules;
using PresenceBackend.Services;
using PresenceBackend.Shared;

namespace PresenceBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IPasswordHasher<User>, BcryptHasher>();
        
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<IContext, DatabaseContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddScoped<DbAccess>();
        builder.Services.AddScoped<IAuthorization, CustomAuthorization>();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Presence Backend",
                Version = "v1",
                Description = "The complete backend for presence. NOTE: The endpoint test functionality does not work, because it is not intended to work for all endpoints"
            });
        });
        
        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        
        app.Run();
    }
}