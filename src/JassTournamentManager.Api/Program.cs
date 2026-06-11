using JassTournamentManager.Application;
using JassTournamentManager.Infrastructure;
using JassTournamentManager.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

ConfigureJwtOptions(builder);

AddServices(builder);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();

static void ConfigureJwtOptions(WebApplicationBuilder builder)
{
    builder.Configuration.AddKeyPerFile(
        directoryPath: "/run/secrets",
        optional: true);

    builder.Services
        .AddOptions<JwtOptions>()
        .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
        .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "JWT issuer is required.")
        .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "JWT audience is required.")
        .Validate(options => !string.IsNullOrWhiteSpace(options.Secret), "JWT secret is required.")
        .Validate(options => options.AccessTokenMinutes > 0, "JWT access token lifetime must be positive.")
        .Validate(options => options.RefreshTokenDays > 0, "JWT refresh token lifetime must be positive.")
        .ValidateOnStart();
}

static void AddServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

    AddAuth(builder);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

static void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

static void AddAuth(WebApplicationBuilder builder)
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();

    builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("RequireSysAdmin", policy => policy.RequireClaim("is_sys_admin", "true"));
}