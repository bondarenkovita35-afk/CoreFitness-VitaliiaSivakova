using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Data;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Lägger till stöd för MVC
builder.Services.AddControllersWithViews();

// Lägger till Infrastructure och databas/Identity
builder.Services.AddInfrastructure(builder.Configuration);

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

// Lägger in testdata i databasen vid start
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    await DbSeeder.SeedTrainingClassesAsync(context);
}

app.Run();