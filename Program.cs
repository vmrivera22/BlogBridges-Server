using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using WebBlog.Data;
using WebBlog.Repositories;
using WebBlog.Repository;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app, app.Environment);

app.Run();

// Add services to the container.
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();

    // Get secrets from key vault.
    var keyVaultEndpoint = new Uri(configuration["VaultKey"]);
    var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
    KeyVaultSecret authority = secretClient.GetSecret("authority");
    KeyVaultSecret audience = secretClient.GetSecret("audience");

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = authority.Value;
        options.Audience = audience.Value;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

    KeyVaultSecret kvs = secretClient.GetSecret("secret");
    services.AddDbContext<DataContext>(o => o.UseSqlServer(kvs.Value));//////));

    services.AddScoped<IRoomsRepository, IMemRoomsRepository>();
    services.AddScoped<IPostsRepository, IMemPostsRepository>();
    services.AddScoped<ICommentsRepository, IMemCommentsRepository>();
    services.AddScoped<IRulesRepository, IMemRulesRepository>();
    services.AddScoped<IUsersRepository, IMemUsersRepository>();
    
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", builder =>
        {
            var corsOrigins = configuration.GetSection("CorsOrigins").Get<string[]>();
            builder.WithOrigins(corsOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
    services.AddSwaggerGen();
}

void Configure(IApplicationBuilder app, IHostEnvironment environment)
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseCors("AllowSpecificOrigin");

    app.UseAuthentication();
    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

