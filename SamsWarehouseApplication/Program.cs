using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SamsWarehouseApplication.Models;
using SamsWarehouseApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ShoppingContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SamsWarehouse")));

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

// Adds the Authentication service using Cookie based scheme.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    // Sets the login view that will be returned when an unauthenticated user tried to access a secured endpoint.
    options.LoginPath= "/Login/Index";
    // Automatically logs the user out after 10 minutes.
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    // Sets the expiration time to reset if the user is still active.
    options.SlidingExpiration = true;
    // Sets the view to be returned if a user is authenticated but not authorised to access the secured endpoint.
    options.AccessDeniedPath = "/Login/AccessDenied";
});

builder.Services.AddScoped<FileUploaderService>();
builder.Services.AddScoped<EncryptionService>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<SanitizationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add(
        "Content-Security-Policy", "default-src 'none'; " +
        "img-src 'self' data:; " +
        "style-src 'self'" +
        "style-src-elem 'self'; " +
        "script-src-elem 'self'; " +
        "connect-src 'self';" +
        "form-action 'self'; " +
        "frame-src youtube.com https://www.youtube.com;");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=63072000; includeSubDomains;");

    await next(context);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
