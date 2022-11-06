using DotNetCore.CAP;
using Newtonsoft.Json;
using PAC.RH.Data;
using PAC.RH.Models;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;

namespace PAC.RH.Consumidores
{
    public class FuncionariosConsumidor : ICapSubscribe
    {
        private readonly ILogger<FuncionariosConsumidor> _logger;
        private readonly RhContext _contexto;

        public FuncionariosConsumidor(ILogger<FuncionariosConsumidor> logger, RhContext contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [CapSubscribe("producao.operario.nome-alterado")]
        public async Task Consumir(OperarioNomeAlteradoMensagem mensagem, CancellationToken cancellationToken)
        {
            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var funcionario = await _contexto.Funcionarios.FindAsync(mensagem.Id);

            if (!FuncionarioExistente(funcionario, mensagem.Id)) return;

            // Como estou tratando como value object e eles são imutáveis então estou instanciando novamente
            var novoNome = new NomeCompleto(mensagem.Nome, mensagem.Apelido);
            funcionario!.AtribuirNovoNome(novoNome, Setor.Producao);

            _contexto.Funcionarios.Update(funcionario);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }
        
        [CapSubscribe("vendas.vendedor.alteracao-info-pessoais")]
        public async Task Consumir(VendedorInfoPessoaisAlteradaMensagem mensagem, CancellationToken cancellationToken)
        {
            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var funcionario = await _contexto.Funcionarios.FindAsync(mensagem.Id);

            if (!FuncionarioExistente(funcionario, mensagem.Id)) return;

            funcionario!.AtribuirNovasInfoPessoais(mensagem.Nome, mensagem.Email, Setor.Vendas);

            _contexto.Funcionarios.Update(funcionario);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }

        private void LogarMensagemProcessada(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem {@tipo} processada com sucesso", mensagem.GetType().Name);

        private void LogarMensagemConsumida(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        private bool FuncionarioExistente(Funcionario? funcionario, Guid identificador)
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
