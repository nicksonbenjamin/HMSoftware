using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ClinicApp.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Org.BouncyCastle.Asn1.X509.Qualified;
using ClinicApp.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Replace 'ClinicAppMySqlDbContext' with your actual DbContext class name, e.g., 'ClinicDbContext'
builder.Services.AddDbContext<ClinicAppMySqlDbContext>(options =>
	options.UseMySql(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
	));

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ApplicationUserViewModel>();
builder.Services.AddScoped<PatientViewModel>();
builder.Services.AddScoped<ProductMasterViewModel>();
builder.Services.AddScoped<LedgerMasterViewModel>();
builder.Services.AddScoped<DescriptionMasterViewModel>();
builder.Services.AddScoped<PatientRegistrationViewModel>(); // << Added PatientRegistration
// Prescription-related ViewModels
builder.Services.AddScoped<DoctorPrescriptionVM>();
builder.Services.AddScoped<DoctorPrescriptionListVM>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessionCheckFilter>();
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

app.UseSession();

app.UseAuthorization();



app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
