using MassTransit;
using PAC.RH.Data;
using PAC.RH.Models;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;

namespace PAC.RH.Consumidores
{
    public class OperarioNomeAlteradoConsumidor : FuncionariosConsumidor, IConsumer<OperarioNomeAlteradoMensagem>
    {
        public OperarioNomeAlteradoConsumidor(
            ILogger<OperarioNomeAlteradoConsumidor> logger,
            RhContext contexto) : base(logger, contexto) { }

        public async Task Consume(ConsumeContext<OperarioNomeAlteradoMensagem> context)
        {
            var mensagem = context.Message;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var funcionario = await _contexto.Funcionarios.FindAsync(mensagem.Id);

            if (!FuncionarioExistente(funcionario, mensagem.Id)) return;

            // Como estou tratando como value object e eles são imutáveis então estou instanciando novamente
            var novoNome = new NomeCompleto(mensagem.Nome, mensagem.Apelido);
            funcionario!.AtribuirNovoNome(novoNome, Setor.Producao);

            _contexto.Funcionarios.Update(funcionario);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}
