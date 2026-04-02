using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Data;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Lägger till stöd för MVC
builder.Services.AddControllersWithViews();

// Lägger till Infrastructure och databas/Identity
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
    {
        // Hämtar Google ClientId från konfiguration
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;

        // Hämtar Google ClientSecret från konfiguration
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

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

    // Lägger in träningspass
    await DbSeeder.SeedTrainingClassesAsync(context);

    // Skapar rollen Admin om den inte finns
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Skapar rollen Member om den inte finns
    if (!await roleManager.RoleExistsAsync("Member"))
    {
        await roleManager.CreateAsync(new IdentityRole("Member"));
    }

    // Lägg din egen inloggningsmail här
    var adminEmail = "moriahek@gmail.com";

    // Hämtar användaren
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    // Ger Admin-roll till användaren
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();