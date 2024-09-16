using EShop.Repository;
using EShop.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using EShop.Service.Interface;
using EShop.Service.Implementation;
using EShop.Repository.Implementation;
using EShop.Repository.Interface;
using NuGet.Protocol.Core.Types;
using EShop.Domain;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;

namespace EShop.Web
{


public class Program
{
        public static async Task Main(string[] args)
        {




var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddDefaultIdentity<PetAdoptionCenterUser>(options => options.SignIn.RequireConfirmedAccount = true)
     .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

// Register repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IAdoptionApplicationRepository, AdoptionApplicationRepository>();
builder.Services.AddScoped<IShelterRepository, ShelterRepository>();



// Register services
builder.Services.AddTransient<IPetService, PetService>();
builder.Services.AddTransient<IAdoptionApplicationService, AdoptionApplicationService>();
builder.Services.AddTransient<IShelterService, ShelterService>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Index";
            });
            // Add controllers with views and JSON settings
            builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
var app = builder.Build();

/*await SeedData.SeedRolesAndAdminAsync(app.Services);*/

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Adopter" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<PetAdoptionCenterUser>>();
                string email = "admin@admin.com";
                string password = "Test1234***";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new PetAdoptionCenterUser
                    {
                        UserName = email,
                        Email = email
                    };

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }


            app.Run();

        }
    }
}