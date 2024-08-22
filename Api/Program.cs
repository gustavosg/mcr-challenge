using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using NLog;
using NLog.Web;

using Api.Configuration;
using Api.Middleware;
using Application.DTO.Configuration;
using Application.Services;
using Core.Entities;
using Infrastructure;
using Api.Extensions;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    builder.Services.AddControllers();

    var connection = builder.Configuration["AppSettings:Database:ConnectionString"];

    KafkaSettings kafkaSettings = new();
    new ConfigureFromConfigurationOptions<KafkaSettings>(
        builder.Configuration.GetSection("AppSettings:Kafka"))
        .Configure(kafkaSettings);

    JwtOptionsSettings jwtOptionsSettings = new();

    new ConfigureFromConfigurationOptions<JwtOptionsSettings>(
        builder.Configuration.GetSection("AppSettings:JwtOptions"))
        .Configure(jwtOptionsSettings);

    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    builder.Services.Configure<JwtOptionsSettings>(builder.Configuration.GetSection("AppSettings:JwtOptions"));
    builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("AppSettings:Kafka"));

    builder.Services.AddHttpContextAccessor();
    builder.Services.SetupDatabase(connection);

    builder.Services.AddHostedService<KafkaApplicationService>();

    builder.Services.AddUnitOfWork();
    builder.Services.AddKafka();
    builder.Services.AddServices();
    builder.Services.AddAutoMapper();

    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
        options.AppendTrailingSlash = true;
    });

    builder.Services.AddIdentity<UserModel, RoleModel>(options =>
    {
        options.User.RequireUniqueEmail = false;
    })
        .AddRoles<RoleModel>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddApiVersioning(v =>
    {
        v.AssumeDefaultVersionWhenUnspecified = true;
        v.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        v.ReportApiVersions = true;
    }).AddApiExplorer(opts =>
    {
        opts.GroupNameFormat = "'v'VVV";
        opts.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddSwagger();

    byte[] key = Encoding.ASCII.GetBytes(jwtOptionsSettings.Secret);

    builder.Services.SetupAuthentication(key);
    builder.Services.SetupAuthorization();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.ApplyMigrations();

    app.UseHttpContextLogging();
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.UseMiddleware<JwtMiddleware>();

    IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

    await RolesCfg.InsertRolesIfDoesntExist(serviceProvider);

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}


public partial class Program { }