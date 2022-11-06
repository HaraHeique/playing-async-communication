using Quartz;

namespace PAC.Producao.Configurations
{
    public static class QuartzExtensions
    {
        public static void AddJobConfig<TJob>(
            this IServiceCollectionQuartzConfigurator quatzConfig,
            IConfiguration configuration) where TJob : IJob
        {
            // Quartz tem três elementos principais: Jobs, Triggers e Schedulers

            // Registrar a injeção de dependência do job no Container de DI da aplicação
            var jobKey = new JobKey(typeof(TJob).Name);
            quatzConfig.AddJob<TJob>(options => options.WithIdentity(jobKey));

            quatzConfig.AddTrigger(config =>
            {
                var cronExpression = configuration.GetValue<string>($"JobScheduler:{jobKey.Name}");
                var triggerName = $"{jobKey.Name}Trigger";

                config.ForJob(jobKey)
                    .WithIdentity(triggerName)
                    .WithCronSchedule(cronExpression); // A cada 15 segundos
            });
        }
    }
}
