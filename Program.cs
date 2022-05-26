using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyPro2;
using MoneyPro2.Data;
using MoneyPro2.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
LoadConfiguration(builder);
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.UseStaticFiles();
//app.UseResponseCompression();
app.Run();

void LoadConfiguration(WebApplicationBuilder builder)
{
    Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = builder.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = builder.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.SmtpConfiguration();
    builder.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder
        .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    var connection = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<MoneyDataContext>(options => options.UseSqlServer(connection));
    builder.Services.AddTransient<TokenService>();
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}