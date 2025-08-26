using ItecwebApp.DAL;
using ItecwebApp.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNet.Security.OAuth.GitHub; // ✅ Needed for GitHub

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Configure authentication & authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
})
// ✅ Google Login
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
// ✅ GitHub Login
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
});

builder.Services.AddAuthorization();

// ✅ Initialize your database helper
var connectionString = builder.Configuration.GetConnectionString("MyConnection");
DatabaseHelper.Init(connectionString);

// ✅ Register DALs for DI
builder.Services.AddScoped<IEditionDAl, EditionDAl>();
builder.Services.AddScoped<IVenueDAl, VenueDAl>();
builder.Services.AddScoped<IEventsDAll, EventsDAll>();
builder.Services.AddScoped<ICommiteesDAl, CommiteesDAl>();
builder.Services.AddScoped<ISponsorsDAl, SponsorsDAl>();
builder.Services.AddScoped<IVendorsDAl, VendorsDAl>();
builder.Services.AddScoped<ICommiteeMembersDal, CommiteeMembersDal>();
builder.Services.AddScoped<IDutiesDAl, DutiesDAl>();
builder.Services.AddScoped<IParticipantsDAL, ParticipantsDAL>();
builder.Services.AddScoped<IUserDAL, UserDAL>();
builder.Services.AddTransient<EmailService>();
var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
