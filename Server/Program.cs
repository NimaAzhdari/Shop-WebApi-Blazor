using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContex>(option =>option.UseSqlite(builder.Configuration.GetConnectionString("MainConnectionString")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => 
{
 options.Password.RequireDigit = false;
 options.Password.RequireLowercase = false;
 options.Password.RequireNonAlphanumeric = false;
 options.Password.RequireUppercase = false;
 options.Password.RequiredLength = 2;
 options.Password.RequiredUniqueChars = 0;
}
)
    .AddEntityFrameworkStores<ApplicationDbContex>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.Name = "authen";
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/Home";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
}
);
builder.Services.AddAuthorization(options=>
{
    options.AddPolicy("AdminOnly",policy =>policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly",policy=>policy.RequireRole("User"));
}
);




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedRoleAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

