using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using SalesWebMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMvcContext"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMvcContext")),
    builder => builder.MigrationsAssembly("SalesWebMvc")));

//Add Seeding Service to application
builder.Services.AddScoped<SeedingService>();

//Add SellerService to aplication
builder.Services.AddScoped<SellerService>();

//Add DepartmentService to aplication
builder.Services.AddScoped<DepartmentService>();

//Add SalesRecordService to aplication
builder.Services.AddScoped<SalesRecordService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
    
    
if (!app.Environment.IsDevelopment())
{// Configure the HTTP request pipeline.
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure locale as US
var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo>() { enUS }
};

app.UseRequestLocalization(localizationOptions);

app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
