using ItecwebApp.DAL;
using ItecwebApp.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// ✅ Add Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Initialize DatabaseHelper BEFORE building the app
var connectionString = builder.Configuration.GetConnectionString("MyConnection");
DatabaseHelper.Init(connectionString);

// ✅ Register DAL in DI container
builder.Services.AddScoped<IEditionDAl, EditionDAl>();
builder.Services.AddScoped<IVenueDAl, VenueDAl>();
builder.Services.AddScoped<IEventsDAll, EventsDAll>();
builder.Services.AddScoped<ICommiteesDAl, CommiteesDAl>();
builder.Services.AddScoped<ISponsorsDAl, SponsorsDAl>();
builder.Services.AddScoped<IVendorsDAl, VendorsDAl>();
builder.Services.AddScoped<ICommiteeMembersDal, CommiteeMembersDal>();
builder.Services.AddScoped<IDutiesDAl, DutiesDAl>();
builder.Services.AddScoped<IParticipantsDAL, ParticipantsDAL>();
builder.Services.AddScoped<IUserDAL, UserDAL>(); // ✅ register UserDAL for AccountController

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

app.UseAuthorization();

// ✅ Enable Session middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
