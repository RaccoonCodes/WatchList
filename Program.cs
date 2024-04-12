using Microsoft.EntityFrameworkCore;
using WatchList.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<InfoDbContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:DataBasePractice"]);
});

//dependency injection so its easier to modify w/o changing too much
builder.Services.AddScoped<IInfoRepository,EFInfoRepository>();
builder.Services.AddScoped<UserServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.UseAuthorization();



SeedData.EnsurePopulated(app);
app.Run();
