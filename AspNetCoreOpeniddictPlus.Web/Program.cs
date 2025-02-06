using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Core.Services;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.InertiaCore.Extensions;
using AspNetCoreOpeniddictPlus.Migrator.Persistence;
using AspNetCoreOpeniddictPlus.Web.BackgroundJobs;
using AspNetCoreOpeniddictPlus.Web.Permissions;
using AspNetCoreOpeniddictPlus.Web.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    }
);

builder.Services.AddOpeniddictPlusDbContext<OpeniddictPlusDbContext>();

builder
    .Services.AddIdentity<OpeniddictPlusUser, OpeniddictPlusRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
        options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
        options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
        options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;
    })
    .AddEntityFrameworkStores<OpeniddictPlusDbContext>()
    .AddDefaultTokenProviders();

builder
    .Services.AddOpeniddictPlusServer<OpeniddictPlusDbContext>()
    .AddEmailSender()
    .AddPermissionManagementService<OpeniddictPlusPermission, OpeniddictPlusDbContext>();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHostedService<ClientSeeder>();
builder.Services.AddHostedService<ApplicationSeeder>();

// Use Hangfire for background jobs in production
builder.Services.AddHostedService<UserDeleteBackgroundJob>();
builder.Services.AddHostedService<NotifyUserDeletionBackgroundJob>();

builder.Services.AddInertia(options =>
{
    options.RootView = "~/Views/Admin.cshtml";
});

builder.Services.AddViteHelper(options =>
{
    options.PublicDirectory = "wwwroot";
    options.BuildDirectory = "build";
    options.ManifestFilename = "manifest.json";
});


builder.Services.AddAuthorization(options =>
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<OpeniddictPlusDbContext>();
    var permissions = dbContext.OpeniddictPlusPermissions.ToList();

    foreach (var permission in permissions)
    {
        options.AddPolicy(permission.Name, policy =>
            policy.Requirements.Add(new PermissionRequirement<string>(permission.Name)));
    }
});

 builder.Services.AddTransient<IAuthorizationHandler, PermissionHandler>();
 builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();
app.UseInertia();
app.Run();
