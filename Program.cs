using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CodeGhar.Data;

var builder = WebApplication.CreateBuilder(args);

// Determine Database Provider (MySQL or SQLite fallback)
var provider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "MySql";
var mySqlConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
var sqliteConnStr = builder.Configuration.GetConnectionString("SqliteConnection") ?? "DataSource=app.db;Cache=Shared";

bool isMySqlActive = false;
if (provider.Equals("MySql", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(mySqlConnStr))
{
    try
    {
        using var testConn = new MySqlConnector.MySqlConnection(mySqlConnStr);
        testConn.Open();
        isMySqlActive = true;
    }
    catch
    {
        isMySqlActive = false;
    }
}

if (isMySqlActive && !string.IsNullOrWhiteSpace(mySqlConnStr))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(
            mySqlConnStr,
            new MySqlServerVersion(new Version(8, 0, 0)),
            mySqlOptions => mySqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)
        ));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(sqliteConnStr));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add State Management (Distributed Memory Cache + Session)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".CodeGhar.Session";
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CodeGhar.Services.IFileService, CodeGhar.Services.FileService>();
builder.Services.AddSingleton<CodeGhar.Services.IDiffService, CodeGhar.Services.DiffService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Preserving HTTP port 5000 as configured
app.UseRouting();

// Enable Session Middleware for State Management
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
        try
        {
            db.Database.ExecuteSqlRaw("UPDATE CodeVersions SET Major = Number WHERE Major = 0");
        }
        catch { }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("Database initialization notice: {Message}", ex.Message);
    }
}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
