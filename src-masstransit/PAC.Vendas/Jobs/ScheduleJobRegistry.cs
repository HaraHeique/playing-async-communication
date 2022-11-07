using FluentScheduler;

namespace PAC.Vendas.Jobs
{
    // Registra o agendador de tarefas
    public class ScheduleJobRegistry : Registry
    {
        public ScheduleJobRegistry(IServiceProvider provider)
        {
            Schedule(() => ObterInstanciaVendedorEventosIntegracaoJob(provider))
                .NonReentrant()
                .ToRunEvery(15).Seconds();
        }

        private static VendedorEventosIntegracaoJob ObterInstanciaVendedorEventosIntegracaoJob(IServiceProvider provider) 
            => provider.CreateScope().ServiceProvider.GetRequiredService<VendedorEventosIntegracaoJob>();
    }
}
