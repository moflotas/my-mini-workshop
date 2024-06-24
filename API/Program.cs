using Autofac.Extensions.DependencyInjection;
using Startup = API.Startup;

Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureWebHostDefaults(
        webBuilder => { webBuilder.UseStartup<Startup>(); })
    .Build()
    .Run();