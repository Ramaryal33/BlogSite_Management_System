using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MurkyPluse.Entities; // Assuming DbContext is in Data folder
using MurkyPluse.Entities;
using MurkyPluse.Models;

var builder = WebApplication.CreateBuilder(args);

// ========== Service Configuration ==========

// File upload settings
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});

// MVC Services
builder.Services.AddControllersWithViews();

// Database Configuration (MUST come first)
builder.Services.AddDbContext<MurkyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure());
});

// Identity Configuration (Uncommented and properly configured)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Simplified password for development (strengthen for production)
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

    // Lockout settings
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MurkyDbContext>()
.AddDefaultTokenProviders();

// Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Auth/SignIn";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.None
        : CookieSecurePolicy.Always;
});

// Database Seeder (Registered AFTER Identity)
builder.Services.AddTransient<DatabaseSeeder>();

// ========== App Build & Middleware ==========
var app = builder.Build();

// Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BlogPost}/{action=Index}/{id?}");

// ========== Database Initialization ==========
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Apply pending migrations
        var dbContext = services.GetRequiredService<MurkyDbContext>();
        await dbContext.Database.MigrateAsync();

        // Seed data only in development
        if (app.Environment.IsDevelopment())
        {
            var seeder = services.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database initialization failed");
    }
}

app.Run();