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
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = configuration.GetValue<string>("ServerAuth:Authority");
        options.Audience = configuration.GetValue<string>("ServerAuth:Audience");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

    services.AddSwaggerGen();
    services.AddScoped<IRoomsRepository, IMemRoomsRepository>();
    services.AddScoped<IPostsRepository, IMemPostsRepository>();
    services.AddScoped<ICommentsRepository, IMemCommentsRepository>();
    services.AddScoped<IRulesRepository, IMemRulesRepository>();
    services.AddScoped<IUsersRepository, IMemUsersRepository>();
    services.AddControllers()
        .AddJsonOptions(options=>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    services.AddDbContext<DataContext>(o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));//////));
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

