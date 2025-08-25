using ItecwebApp.DAL;
using ItecwebApp.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Register HttpContextAccessor (optional, but useful for DI)
builder.Services.AddHttpContextAccessor();

// ✅ Add memory cache (required for session)
builder.Services.AddDistributedMemoryCache();

// ✅ Add session support (optional, only if you use HttpContext.Session)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Configure authentication & authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";           // redirect if not logged in
        options.AccessDeniedPath = "/Account/AccessDenied"; // redirect if role denied
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;              // refresh timeout on activity
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// ✅ Enable session middleware (optional)
app.UseSession();

// Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
