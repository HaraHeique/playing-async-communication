using MassTransit;
using PAC.RH.Data;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;

namespace PAC.RH.Consumidores
{
    public class VendedorInfoPessoaisAlteradaConsumidor : FuncionariosConsumidor, IConsumer<VendedorInfoPessoaisAlteradaMensagem>
    {
        public VendedorInfoPessoaisAlteradaConsumidor(
            ILogger<VendedorInfoPessoaisAlteradaConsumidor> logger,
            RhContext contexto) : base(logger, contexto) { }

        public async Task Consume(ConsumeContext<VendedorInfoPessoaisAlteradaMensagem> context)
        {
            var mensagem = context.Message;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var funcionario = await _contexto.Funcionarios.FindAsync(mensagem.Id);

            if (!FuncionarioExistente(funcionario, mensagem.Id)) return;

            funcionario!.AtribuirNovasInfoPessoais(mensagem.Nome, mensagem.Email, Setor.Vendas);

            _contexto.Funcionarios.Update(funcionario);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}
