using API.Configuration;
using API.Configuration.ExecutionContext;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.Application.Configuration;
using Microsoft.EntityFrameworkCore;
using Modules.Events.Application.Contracts;
using Modules.Events.Infrastructure;
using static Modules.Events.Infrastructure.Configuration.Startup;
using Modules.Sport.Application.Contracts;
using Modules.Sport.Infrastructure;
using static Modules.Sport.Infrastructure.Configuration.Startup;
using Modules.UserAccess.Application.Contracts;
using Modules.UserAccess.Infrastructure;
using static Modules.UserAccess.Infrastructure.Configuration.Startup;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using Logger = Serilog.Core.Logger;

namespace API;

public class Startup
{
    internal static IWebHostEnvironment Env = default!;
    private readonly Secrets _secrets;
    private readonly Settings _settings;
    private readonly Logger _loggerBuilder;
    
    private IContainer _userAccessContainer = default!;
    private IContainer _sportContainer = default!;
    private IContainer _eventsContainer = default!;

    public Startup(IWebHostEnvironment env)
    {
        Env = env;
        _loggerBuilder = Configuration.Logger.CreateLogger();

        var developmentSecretsLocation =
            Path.GetDirectoryName(Directory.GetCurrentDirectory())
            + Path.DirectorySeparatorChar
            + "secrets.json";

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
            .AddJsonFile(developmentSecretsLocation, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        _secrets = configuration.GetSection("Secrets").Get<Secrets>()!;
        _settings = configuration.GetSection("Settings").Get<Settings>()!;
    }

    public void ConfigureServices(IServiceCollection s)
    {
        s.InitRouting();
        s.InitSwagger();
        s.InitAuthentication(_secrets);
        s.InitAuthorization();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        #region ForMigrations

        builder.Register(c =>
                new UserAccessContext(new DbContextOptionsBuilder()
                    .UseNpgsql(_secrets.Database.ConnectionString)
                    .Options)
            )
            .AsSelf()
            .InstancePerLifetimeScope();
        builder.Register(c =>
                new SportContext(new DbContextOptionsBuilder()
                    .UseNpgsql(_secrets.Database.ConnectionString)
                    .Options)
            )
            .AsSelf()
            .InstancePerLifetimeScope();
        builder.Register(c =>
                new EventsContext(new DbContextOptionsBuilder()
                    .UseNpgsql(_secrets.Database.ConnectionString)
                    .Options)
            )
            .AsSelf()
            .InstancePerLifetimeScope();

        #endregion

        builder.Register(c => new UserAccessModule(_userAccessContainer))
            .As<IUserAccessModule>()
            .InstancePerLifetimeScope();
        builder.Register(c => new SportModule(_sportContainer))
            .As<ISportModule>()
            .InstancePerLifetimeScope();
        builder.Register(c => new EventsModule(_eventsContainer))
            .As<IEventsModule>()
            .InstancePerLifetimeScope();

        builder.RegisterInstance(_settings);
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        var container = app.ApplicationServices.GetAutofacRoot();

        var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
        var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

        _userAccessContainer = InitUserAccessModule(_secrets, _loggerBuilder, executionContextAccessor);
        _sportContainer = InitSportModule(_secrets, _loggerBuilder, executionContextAccessor);
        _eventsContainer = InitEventsModule(_secrets, _loggerBuilder, executionContextAccessor);

        app.InitRouting();
        app.InitSwagger();
    }
}