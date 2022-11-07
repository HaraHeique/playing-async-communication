using Newtonsoft.Json;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;
using PAC.Vendas.Data;
using PAC.Vendas.Models.Domain;

namespace PAC.Vendas.Consumidores
{
    public abstract class VendedoresConsumidor
    {
        protected readonly ILogger<VendedoresConsumidor> _logger;
        protected readonly VendasContext _contexto;

        public VendedoresConsumidor(ILogger<VendedoresConsumidor> logger, VendasContext contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        protected void LogarMensagemProcessada(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem {@tipo} processada com sucesso", mensagem.GetType().Name);

        protected void LogarMensagemConsumida(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        protected static bool SetorInvalido(Setor setor) => setor != Setor.Vendas;

        protected bool VendedorExistente(Vendedor? vendedor, Guid identificador)
        {
            if (vendedor is null)
            {
                _logger.LogError("Vendedor com Id {@id} não encontrado na base de dados", identificador);
                return false;
            }

            return true;
        }
    }
}
