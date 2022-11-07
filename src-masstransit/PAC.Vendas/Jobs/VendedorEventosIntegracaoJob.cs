using FluentScheduler;
using MassTransit;
using Newtonsoft.Json;
using PAC.Shared.Mensagens;
using System.Collections.Concurrent;

namespace PAC.Vendas.Jobs
{
    public class VendedorEventosIntegracaoJob : IAsyncJob
    {
        private readonly ILogger<VendedorEventosIntegracaoJob> _logger;
        private readonly ConcurrentQueue<IntegracaoMensagem> _filaProcessos;
        private readonly IPublishEndpoint _produtor;

        public VendedorEventosIntegracaoJob(
            ILogger<VendedorEventosIntegracaoJob> logger, 
            ConcurrentQueue<IntegracaoMensagem> filaProcessos,
            IPublishEndpoint produtor)
        {
            _logger = logger;
            _filaProcessos = filaProcessos;
            _produtor = produtor;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Início do job de publicação da mensagem de integração");

            await PublicarMensagem();

            _logger.LogInformation("Finalização do job de publicação da mensagem de integração");
        }

        private async Task PublicarMensagem()
        {
            if (_filaProcessos.IsEmpty) return;

            var mensagem = ObterProximaMensagem();

            if (mensagem is null) return;

            LogarInformacoesMensagemConsumida(mensagem);

            await _produtor.Publish(mensagem, mensagem.GetType());

            RemoverProximaMensagem();
        }

        private void LogarInformacoesMensagemConsumida(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        private IntegracaoMensagem? ObterProximaMensagem()
        {
            if (!_filaProcessos.TryPeek(out var mensagem))
            {
                _logger.LogError("Não foi possível obter mensagem {@tipoMensagem} para da fila de processos", mensagem?.GetType().Name);
                return null;
            }

            return mensagem;
        }

        private void RemoverProximaMensagem()
        {
            // Remove mensagem da fila de processos em memória
            if (!_filaProcessos.TryDequeue(out var mensagem))
            {
                _logger.LogError("Falha na remoção da mensagem {@tipoMensagem} da fila de processos", mensagem?.GetType().Name);
            }
        }
    }
}
