using Microsoft.EntityFrameworkCore;
using MyDigitalCv.Data;
using MyDigitalCv.Services;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Repositories;
using MyDigitalCv.Helpers;
using MyDigitalCv.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options => 
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
var app = builder.Build();


if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
	await Seed.SeedUsersAndRolesAsync(app);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Blog/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Blog}/{action=Index}/{id?}");

app.Run();
