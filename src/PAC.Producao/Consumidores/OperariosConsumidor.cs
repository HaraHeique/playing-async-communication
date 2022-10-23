using DotNetCore.CAP;
using Newtonsoft.Json;
using PAC.Shared.Mensagens;

namespace PAC.Producao.Consumidores
{
    public class OperariosConsumidor : ICapSubscribe
    {
        private readonly ILogger<OperariosConsumidor> _logger;

        public OperariosConsumidor(ILogger<OperariosConsumidor> logger)
        {
            _logger = logger;
        }

        [CapSubscribe("funcionario.registrado")]
        public void Consumir(FuncionarioProducaoRegistradoMensagem mensagem)
        {
            _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));
        }
    }
}
