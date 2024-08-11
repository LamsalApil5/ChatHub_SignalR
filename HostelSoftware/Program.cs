using Database;
using Entity.Tables;
using HostelSoftware.Models;
using HostelSoftware.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework and Identity services
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Users, IdentityRole<int>>()
    .AddEntityFrameworkStores<MyDBContext>()
    .AddDefaultTokenProviders();

// Configure SignalR
builder.Services.AddSignalR();

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Configure session management
builder.Services.AddDistributedMemoryCache(); // Adds in-memory cache for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie accessible only via HTTP
    options.Cookie.IsEssential = true; // Make the session cookie essential
});



var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Use HTTP Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Use session middleware
app.UseSession();

// Use custom middleware for token validation
app.UseMiddleware<TokenValidationMiddleware>(); // Ensure this middleware is correctly implemented

// Map endpoints
app.MapHub<ChatHub>("/chatHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
