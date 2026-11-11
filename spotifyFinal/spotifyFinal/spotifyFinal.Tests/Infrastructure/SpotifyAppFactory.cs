using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Data;
using Service.Helpers.Enum;
using spotifyFinal.Controllers;

namespace spotifyFinal.Tests.Infrastructure;

/// <summary>
/// Starts the real Spotify web app in memory for each test class.
/// The only thing replaced is the database: SQL Server -> EF Core InMemory,
/// so the tests run on any machine with just `dotnet test`.
/// </summary>
public class SpotifyAppFactory : WebApplicationFactory<HomeController>
{
    // Unique DB per factory, so test classes never share data.
    private readonly string _dbName = "SpotifyTests_" + Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // Remove the SQL Server registration made in Program.cs ...
            var sqlServerOptions = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(sqlServerOptions);

            // ... and register the same DbContext on top of an in-memory database.
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(_dbName));
        });
    }

    /// <summary>Creates roles and the test users described in <see cref="TestUsers"/>.</summary>
    public async Task SeedAsync()
    {
        using var scope = Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var role in Enum.GetNames<RoleEnums>())
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        await CreateUserAsync(userManager, TestUsers.Confirmed, emailConfirmed: true);
        await CreateUserAsync(userManager, TestUsers.Unconfirmed, emailConfirmed: false);
    }

    private static async Task CreateUserAsync(UserManager<AppUser> userManager, TestUser data, bool emailConfirmed)
    {
        if (await userManager.FindByNameAsync(data.UserName) is not null) return;

        var user = new AppUser
        {
            UserName = data.UserName,
            Email = data.Email,
            Fullname = data.FullName,
            EmailConfirmed = emailConfirmed
        };
        var result = await userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException("Seeding failed: " + string.Join("; ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, nameof(RoleEnums.Member));
    }

    /// <summary>
    /// HttpClient that keeps cookies (needed for antiforgery + login cookie)
    /// and does NOT follow redirects, so tests can assert on 302 + Location.
    /// </summary>
    public HttpClient CreateBrowserLikeClient() => CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false,
        HandleCookies = true,
        BaseAddress = new Uri("https://localhost")
    });

    /// <summary>Checks the database directly — used to prove a user was (not) created.</summary>
    public async Task<bool> UserExistsAsync(string userName)
    {
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        return await userManager.FindByNameAsync(userName) is not null;
    }
}
