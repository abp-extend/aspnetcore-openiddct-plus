using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.Web.Persistence;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddOpeniddictPlusDbContext<OpeniddictPlusDbContext>();

builder.Services.AddIdentity<OpeniddictPlusUser, OpeniddictPlusRole>()
    .AddEntityFrameworkStores<OpeniddictPlusDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOpeniddictPlusServer<OpeniddictPlusDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();
app.MapControllers();
app.Run();