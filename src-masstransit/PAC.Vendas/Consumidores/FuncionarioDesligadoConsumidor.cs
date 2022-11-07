using MassTransit;
using PAC.Shared.Mensagens;
using PAC.Vendas.Data;

namespace PAC.Vendas.Consumidores
{
    public class FuncionarioDesligadoConsumidor : VendedoresConsumidor, IConsumer<FuncionarioDesligadoMensagem>
    {
        public FuncionarioDesligadoConsumidor(
            ILogger<FuncionarioDesligadoConsumidor> logger,
            VendasContext producaoContext) : base(logger, producaoContext) { }

        public async Task Consume(ConsumeContext<FuncionarioDesligadoMensagem> context)
        {
            var mensagem = context.Message;

            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = await _contexto.Vendedores.FindAsync(mensagem.Id);

            if (!VendedorExistente(operario, mensagem.Id)) return;

            operario!.Desligar();
            _contexto.Vendedores.Update(operario);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}
