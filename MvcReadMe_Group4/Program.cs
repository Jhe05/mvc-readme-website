using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcReadMe_Group4.Data;
using MvcReadMe_Group4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using MvcReadMe_Group4.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add DbContext
builder.Services.AddDbContext<MvcReadMe_Group4Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MvcReadMe_Group4Context") ?? throw new InvalidOperationException("Connection string 'MvcReadMe_Group4Context' not found.")));

// Provide IPasswordHasher<User> for hashing passwords in controllers
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<MvcReadMe_Group4.Models.User>, Microsoft.AspNetCore.Identity.PasswordHasher<MvcReadMe_Group4.Models.User>>();

// Add authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Call InitializeData to reset data on each app startup
    InitializeData.Initialize(services);

    // Ensure Favorites table exists (safe to run even if migrations haven't been applied)
    try
    {
        var ctx = services.GetRequiredService<MvcReadMe_Group4Context>();
        ctx.Database.ExecuteSqlRaw(@"CREATE TABLE IF NOT EXISTS Favorites (
            FavoriteId INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            BookId INTEGER NOT NULL,
            CreatedAt TEXT NOT NULL
        );");
    }
    catch (Exception ex)
    {
        // Log if required - can't inject logger here easily; swallow to avoid blocking startup
        System.Console.WriteLine("Warning: could not ensure Favorites table exists: " + ex.Message);
    }

    // Ensure Comments table exists so the app can persist comments even when EF migrations
    // haven't been applied (useful for local/dev deploys where running full migrations fails).
    try
    {
        var ctx2 = services.GetRequiredService<MvcReadMe_Group4Context>();
        ctx2.Database.ExecuteSqlRaw(@"CREATE TABLE IF NOT EXISTS Comments (
            CommentId INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            BookId INTEGER NOT NULL,
            CommentText TEXT,
            CreatedAt TEXT NOT NULL,
            IsHidden INTEGER NOT NULL DEFAULT 0
        );");
        // Optionally create an index to speed reads
        ctx2.Database.ExecuteSqlRaw(@"CREATE INDEX IF NOT EXISTS IX_Comments_BookId ON Comments (BookId);");
    }
    catch (Exception ex)
    {
        System.Console.WriteLine("Warning: could not ensure Comments table exists: " + ex.Message);
    }
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable session before authentication
app.UseSession();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map SignalR hub
app.MapHub<NotificationHub>("/notificationHub");

app.MapStaticAssets();


// Default landing page

app.MapGet("/", () => Results.Redirect("/Account/Login"));
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "adminManageBooks",
    pattern: "Admin/ManageBooks/{action=Index}/{id?}",
    defaults: new { controller = "ManageBooks", action = "Index" }
);

app.MapControllerRoute(
    name: "adminManageUsers",
    pattern: "Admin/ManageUsers/{action=Index}/{id?}",
    defaults: new { controller = "ManageUsers", action = "Index" }
);

// Redirects
app.MapGet("/manageusers/index", () => Results.Redirect("/Admin/ManageUsers"));
app.MapGet("/managebooks/index", () => Results.Redirect("/Admin/ManageBooks"));

app.Run();
