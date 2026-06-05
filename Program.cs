using HakatonServer.Data;
using HakatonServer.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;  

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7088/",
            ValidAudience = "https://localhost:7088/",
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes("MegaSuperSecret2006HakatonAVIATECH"))
        };
    });

builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("MySql");
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
    
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseAuthorization();
app.MapHub<NewsHub>("newsHub");

app.Run();
