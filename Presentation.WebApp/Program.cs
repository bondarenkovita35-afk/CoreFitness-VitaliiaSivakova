using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Persistence.Contexts;
using Presentation.WebApp.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Lägger till stöd för MVC
builder.Services.AddControllersWithViews();

// Lägger till Infrastructure och databas/Identity
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Hämtar Google-inställningar från konfiguration
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

// Lägger bara till Google-inloggning om nycklar finns
if (!string.IsNullOrWhiteSpace(googleClientId) &&
    !string.IsNullOrWhiteSpace(googleClientSecret))
{
    builder.Services
        .AddAuthentication()
        .AddGoogle(options =>
        {
            // Google ClientId
            options.ClientId = googleClientId;

            // Google ClientSecret
            options.ClientSecret = googleClientSecret;
        });
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Aktiverar inloggning och behörighet
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Lägger in testdata och roller vid start
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await DbSeeder.SeedTrainingClassesAsync(context);

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Member"))
        await roleManager.CreateAsync(new IdentityRole("Member"));

    var adminEmail = "admin@corefitness.com";
    var adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    // Skapar admin-användare om den inte finns
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,

            FirstName = "Admin",
            LastName = "CoreFitness"

        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createResult.Succeeded)
        {
            foreach (var error in createResult.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }

    // Lägger till Admin-roll om den saknas
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();