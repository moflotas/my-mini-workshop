using Autofac;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BuildingBlocks.Infrastructure.Configuration.Database;

public class DatabaseStartup(Func<ILifetimeScope> createScope)
{
    public static void Initialize(Func<ILifetimeScope> createScope)
    {
        using var scope = createScope();
        var logger = scope.Resolve<ILogger>();
        var dbContext = scope.Resolve<DbContext>();

        logger.Information("Database initialization started");

        dbContext.Database.Migrate();

        logger.Information("Database initialization completed");
    }
}