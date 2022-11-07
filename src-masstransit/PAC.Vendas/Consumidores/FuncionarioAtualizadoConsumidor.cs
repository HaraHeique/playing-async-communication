using MassTransit;
using PAC.Shared.Mensagens;
using PAC.Vendas.Data;

namespace PAC.Vendas.Consumidores
{
    public class FuncionarioAtualizadoConsumidor : VendedoresConsumidor, IConsumer<FuncionarioAtualizadoMensagem>
    {
        public FuncionarioAtualizadoConsumidor(
            ILogger<FuncionarioAtualizadoConsumidor> logger,
            VendasContext producaoContext) : base(logger, producaoContext) { }

        public async Task Consume(ConsumeContext<FuncionarioAtualizadoMensagem> context)
        {
            var mensagem = context.Message;

            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var vendedor = await _contexto.Vendedores.FindAsync(mensagem.Id);

            if (!VendedorExistente(vendedor, mensagem.Id)) return;

            vendedor!.AlterarNome(mensagem.Nome);
            _contexto.Vendedores.Update(vendedor);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}
