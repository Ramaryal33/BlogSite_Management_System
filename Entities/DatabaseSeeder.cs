using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MurkyPluse.Entities;

public class DatabaseSeeder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<DatabaseSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            await SeedRoles();
            await SeedAdminUser();
            await SeedRegularUser();

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw; // Re-throw to make the application fail fast if seeding fails
        }
    }

    private async Task SeedRoles()
    {
        string[] roles = { "Admin", "User", "Editor" }; // Added Editor role
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                _logger.LogInformation("Creating role: {Role}", role);
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task SeedAdminUser()
    {
        const string adminEmail = "admin@example.com";
        const string adminPassword = "Admin@123"; // In production, use a secure password from config

        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            _logger.LogInformation("Creating admin user");
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true // Bypass email confirmation for seeding
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(adminUser, "Editor"); // Admin gets Editor role too
            }
            else
            {
                _logger.LogError("Failed to create admin user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task SeedRegularUser()
    {
        const string userEmail = "user@example.com";
        const string userPassword = "User@123"; // In production, use a secure password from config

        if (await _userManager.FindByEmailAsync(userEmail) == null)
        {
            _logger.LogInformation("Creating regular user");
            var regularUser = new IdentityUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true // Bypass email confirmation for seeding
            };

            var result = await _userManager.CreateAsync(regularUser, userPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(regularUser, "User");
            }
            else
            {
                _logger.LogError("Failed to create regular user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}