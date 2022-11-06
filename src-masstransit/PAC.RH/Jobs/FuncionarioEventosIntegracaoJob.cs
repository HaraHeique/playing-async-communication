using Hangfire;
using MassTransit;
using Newtonsoft.Json;
using PAC.Shared.Mensagens;
using System.Collections.Concurrent;

#nullable disable
namespace PAC.RH.Jobs
{
    public class FuncionarioEventosIntegracaoJob
    {
        private readonly ILogger<FuncionarioEventosIntegracaoJob> _logger;
        private readonly ConcurrentQueue<IntegracaoMensagem> _filaProcessos;
        private readonly IPublishEndpoint _produtor;

        public FuncionarioEventosIntegracaoJob(
            ILogger<FuncionarioEventosIntegracaoJob> logger,
            ConcurrentQueue<IntegracaoMensagem> filaProcessos,
            IPublishEndpoint produtor)
        {
            _logger = logger;
            _filaProcessos = filaProcessos;
            _produtor = produtor;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 60)]
        public async Task Executar()
        {
            _logger.LogInformation("Início do job de publicação da mensagem de integração");

            await PublicarMensagem();

            _logger.LogInformation("Finalização do job de publicação da mensagem de integração");
        }

        private async Task PublicarMensagem()
        {
            if (_filaProcessos.IsEmpty) return;

            var mensagem = ObterProximaMensagem(_filaProcessos);

            if (mensagem is null) return;

            _logger.LogInformation("Mensagem - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

            // CAP usa Outbox pattern, ou seja, garante sempre o envio da mensagem para o broker e usa políticas de retry caso ocorram falhas (olhar na docs)
            await _produtor.Publish(mensagem);

            RemoverProximaMensagem(_filaProcessos);
        }

        private IntegracaoMensagem ObterProximaMensagem(ConcurrentQueue<IntegracaoMensagem> filaProcessos)
        {
            if (!filaProcessos.TryPeek(out var mensagem))
            {
                _logger.LogError("Não foi possível obter mensagem {@tipoMensagem} para da fila de processos", mensagem.GetType().Name);
                return null;
            }

            return mensagem;
        }

        private void RemoverProximaMensagem(ConcurrentQueue<IntegracaoMensagem> filaProcessos)
        {
            // Remove mensagem da fila de processos em memória
            if (!filaProcessos.TryDequeue(out var mensagem))
            {
                _logger.LogError("Falha na remoção da mensagem {@tipoMensagem} da fila de processos", mensagem.GetType().Name);
            }
        }
    }
}
