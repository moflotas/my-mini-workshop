using System.Collections.Specialized;
using Autofac;
using BuildingBlocks.Infrastructure.Inbox;
using BuildingBlocks.Infrastructure.InternalCommands;
using BuildingBlocks.Infrastructure.Outbox;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace BuildingBlocks.Infrastructure.Configuration.Quartz;

public static class QuartzStartup
{
    private static readonly TimeSpan ProcessingInterval = TimeSpan.FromSeconds(2);

    public static IScheduler Initialize(Func<ILifetimeScope> createScope, string instanceName)
    {
        var logger = createScope().Resolve<ILogger>();

        logger.Information("Quartz starting...");

        var schedulerConfiguration = new NameValueCollection { { "quartz.scheduler.instanceName", instanceName } };

        var schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        var scheduler = schedulerFactory.GetScheduler().Result;
        scheduler.JobFactory = new AutofacJobFactory(createScope);

        // LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));

        scheduler.Start().Wait();

        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
        var trigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x.WithInterval(ProcessingInterval).RepeatForever())
            .Build();
        scheduler.ScheduleJob(processOutboxJob, trigger).Wait();

        var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();
        var processInboxTrigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x.WithInterval(ProcessingInterval).RepeatForever())
            .Build();
        scheduler.ScheduleJob(processInboxJob, processInboxTrigger).Wait();

        var processInternalJob = JobBuilder.Create<ProcessInternalJob>().Build();
        var processInternalTrigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x.WithInterval(ProcessingInterval).RepeatForever())
            .Build();
        scheduler.ScheduleJob(processInternalJob, processInternalTrigger).Wait();

        logger.Information("Base quartz jobs scheduled.");

        return scheduler;
    }
}