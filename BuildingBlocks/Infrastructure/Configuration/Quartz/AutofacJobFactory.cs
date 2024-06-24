using Autofac;
using Quartz;
using Quartz.Spi;

namespace BuildingBlocks.Infrastructure.Configuration.Quartz;

public class AutofacJobFactory(Func<ILifetimeScope> createScope) : IJobFactory
{
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return (IJob)createScope().Resolve(bundle.JobDetail.JobType);
    }

    public void ReturnJob(IJob job)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        (job as IDisposable)?.Dispose();
    }
}