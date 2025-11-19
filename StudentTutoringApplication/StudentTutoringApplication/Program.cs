using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentTutoringApplication.Data;
using StudentTutoringApplication.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the scaffolded TutoringContext (it will use the connection string defined inside its own OnConfiguring method).
builder.Services.AddDbContext<TutoringContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// 尝试种子数据，但不阻止应用启动
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await SeedRolesAndUsersAsync(services);
    }
}
catch (Exception ex)
{
    // 记录错误但不阻止应用启动
    Console.WriteLine($"⚠️ Error seeding database: {ex.Message}");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();


app.Run();


// ---------------------
// Seed Method
// ---------------------
static async Task SeedRolesAndUsersAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "Tutor", "Student" };

    // Ensure all roles exist
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
            {
                Console.WriteLine($"⚠️ Failed to create role {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    // Define users to seed
    var users = new[]
    {
        new { Email = "admin@devtest.com", Password = "Password123!", Role = "Admin" },
        new { Email = "tutor@devtest.com", Password = "Password123!", Role = "Tutor" },
        new { Email = "student@devtest.com", Password = "Password123!", Role = "Student" }
    };

    // Create each user safely
    foreach (var u in users)
    {
        var existingUser = await userManager.FindByEmailAsync(u.Email);
        if (existingUser == null)
        {
            var newUser = new IdentityUser
            {
                UserName = u.Email,
                Email = u.Email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(newUser, u.Password);
            if (!createResult.Succeeded)
            {
                Console.WriteLine($"⚠️ Failed to create user {u.Email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                continue; // skip assigning roles if user creation failed
            }

            // Assign role
            await userManager.AddToRoleAsync(newUser, u.Role);
            Console.WriteLine($"✅ Created {u.Role} user: {u.Email}");
        }
        else
        {
            // Ensure the role is assigned (in case it wasn’t)
            if (!await userManager.IsInRoleAsync(existingUser, u.Role))
            {
                await userManager.AddToRoleAsync(existingUser, u.Role);
                Console.WriteLine($"🔄 Added missing role '{u.Role}' to existing user {u.Email}");
            }
        }
    }



    Console.WriteLine("🎉 Role and user seeding complete!");
}


