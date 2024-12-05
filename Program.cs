using System.Text.Json.Serialization;
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
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        
        builder.Services.AddSingleton<IPasswordHasher<User>, BcryptHasher>();
        
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<IContext, DatabaseContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddScoped<DbAccess>();
        builder.Services.AddScoped<IAuthorization, CustomAuthorization>();
        builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
        builder.Services.AddTransient<MailService>();
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
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        try
        {
            using (var scope = app.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetService<IContext>()!)
            {
                db.Database.EnsureCreated();
                db.Database.Migrate();
                if (await db.Users.CountAsync() == 0)
                {
                    Console.WriteLine("No Users Found");
                    var user = new User();
                    user.Username = "admin";
                    user.Email = "admin@rathje-vt.de";
                    user.UserRoles = new List<UserRole>() { UserRole.Admin };
                    user.FirstName = "Rathje";
                    user.LastName = "VT";
                    user.Password = scope.ServiceProvider.GetService<IPasswordHasher<User>>()
                        .HashPassword(user, "Admin123");
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Migration executed.");
        }
        
        app.Run();
    }
}