using DotNetCore.CAP;
using Newtonsoft.Json;
using PAC.Producao.Data;
using PAC.Producao.Models;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;

namespace PAC.Producao.Consumidores
{
    public class OperariosConsumidor : ICapSubscribe
    {
        private readonly ILogger<OperariosConsumidor> _logger;
        private readonly ProducaoContext _contexto;

        public OperariosConsumidor(ILogger<OperariosConsumidor> logger, ProducaoContext producaoContext)
        {
            _logger = logger;
            _contexto = producaoContext;
        }

        [CapSubscribe("funcionario.registrado")]
        public async Task Consumir(FuncionarioProducaoRegistradoMensagem mensagem, CancellationToken cancellationToken)
        {
            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = new Operario(mensagem.Id, mensagem.Nome, mensagem.Apelido);

            await _contexto.Operarios.AddAsync(operario, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }
        
        [CapSubscribe("funcionario.atualizado")]
        public async Task Consumir(FuncionarioAtualizadoMensagem mensagem, CancellationToken cancellationToken)
        {
            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = await _contexto.Operarios.FindAsync(mensagem.Id);

            if (!OperarioExistente(operario, mensagem.Id)) return;

            operario!.AlterarNome(mensagem.Nome);
            _contexto.Operarios.Update(operario);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }
        
        [CapSubscribe("funcionario.desligado")]
        public async Task Consumir(FuncionarioDesligadoMensagem mensagem, CancellationToken cancellationToken)
        {
            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = await _contexto.Operarios.FindAsync(mensagem.Id);

            if (!OperarioExistente(operario, mensagem.Id)) return;

            operario!.Desligar();
            _contexto.Operarios.Update(operario);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }

        private void LogarMensagemProcessada(IntegracaoMensagem mensagem) 
            => _logger.LogInformation("Mensagem {@tipo} processada com sucesso", mensagem.GetType().Name);

        private void LogarMensagemConsumida(IntegracaoMensagem mensagem) 
            => _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        private static bool SetorInvalido(Setor setor) 
            => setor != Setor.Producao;

        private bool OperarioExistente(Operario? operario, Guid identificador)
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
