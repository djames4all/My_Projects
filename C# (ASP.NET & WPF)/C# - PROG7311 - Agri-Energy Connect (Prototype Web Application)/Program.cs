using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_Part2.Data;

namespace Prog7311_POE_Part2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                // Add Support for the two roles (Farmer and Employee)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            //Any new account will be assigned as a Farmer by default
            // Reference: Jr Innocent Manganyi Booster Session(Online)
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnSignedIn = async context =>
                {
                    var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();

                    // Get the user account
                    var user = await userManager.GetUserAsync(context.Principal);

                    // Assign "Farmer" role if user has neither "Employee" nor "Farmer"
                    if (user != null && !await userManager.IsInRoleAsync(user, "Employee") && !await userManager.IsInRoleAsync(user, "Farmer"))
                    {
                        await userManager.AddToRoleAsync(user, "Farmer");
                    }
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            // Create a temp scope to access services (user managers and roles)
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // UserManager access to manage users
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Declare the application roles
                string[] roles = { "Employee", "Farmer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create a Default Administrator Account
                string adminEmail = "admin@AgriEnergy.co.za";
                string adminPassword = "Admin@123!";

                //Check if Administrator Account exist
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                //If Administrator Account do not exist 
                if (adminUser != null)
                {
                    var user = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                    };

                    //Create User with a Password to the system
                    var result = await userManager.CreateAsync(user, adminPassword);


                    //On a successful creation, Assign the role of Employee

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Employee");
                    }
                }
            }



            app.Run();
        }
    }
}
