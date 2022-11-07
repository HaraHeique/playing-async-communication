using Newtonsoft.Json;
using PAC.Producao.Data;
using PAC.Producao.Models;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;

namespace PAC.Producao.Consumidores
{
    public class OperariosConsumidor
    {
        protected readonly ILogger<OperariosConsumidor> _logger;
        protected readonly ProducaoContext _contexto;

        public OperariosConsumidor(ILogger<OperariosConsumidor> logger, ProducaoContext producaoContext)
        {
            _logger = logger;
            _contexto = producaoContext;
        }

        protected void LogarMensagemProcessada(IntegracaoMensagem mensagem) 
            => _logger.LogInformation("Mensagem {@tipo} processada com sucesso", mensagem.GetType().Name);

        protected void LogarMensagemConsumida(IntegracaoMensagem mensagem) 
            => _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        protected static bool SetorInvalido(Setor setor) 
            => setor != Setor.Producao;

        protected bool OperarioExistente(Operario? operario, Guid identificador)
        {
            if (operario is null)
            {
                _logger.LogError("Operário com Id {@id} não encontrado na base de dados", identificador);
                return false;
            }

            return true;
        }
    }
}
