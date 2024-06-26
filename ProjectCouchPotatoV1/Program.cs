using ProjectCouchPotatoV1.Search;
using ProjectCouchPotatoV1.Models;
using ProjectCouchPotatoV1.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using ProjectCouchPotatoV1.Data;
using ProjectCouchPotatoV1.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(configuration => configuration
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ITMDBService, TMDBService>();
builder.Services.AddScoped<ITMDBService, TMDBService>();
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});
builder.Services.AddMvc(options => options.ModelValidatorProviders.Clear());
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddDbContext<MovieDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MovieDbContext>();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHangfireServer();
app.UseHangfireDashboard();
app.UseFileServer();
app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});


app.Run();