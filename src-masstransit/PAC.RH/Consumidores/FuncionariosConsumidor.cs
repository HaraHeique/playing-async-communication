using Newtonsoft.Json;
using PAC.RH.Data;
using PAC.RH.Models;
using PAC.Shared.Mensagens;

namespace PAC.RH.Consumidores
{
    public abstract class FuncionariosConsumidor
    {
        protected readonly ILogger<FuncionariosConsumidor> _logger;
        protected readonly RhContext _contexto;

        public FuncionariosConsumidor(ILogger<FuncionariosConsumidor> logger, RhContext contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        protected void LogarMensagemProcessada(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem {@tipo} processada com sucesso", mensagem.GetType().Name);

        protected void LogarMensagemConsumida(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        protected bool FuncionarioExistente(Funcionario? funcionario, Guid identificador)
        {
            if (funcionario is null)
            {
                _logger.LogError("Funcionário com Id {@id} não encontrado na base de dados", identificador);
                return false;
            }

            return true;
        }
    }
}
