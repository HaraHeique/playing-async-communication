using MassTransit;
using PAC.Shared.Mensagens;
using PAC.Vendas.Data;
using PAC.Vendas.Models.Domain;

namespace PAC.Vendas.Consumidores
{
    public class FuncionarioRegistradoConsumidor : VendedoresConsumidor, IConsumer<FuncionarioVendasRegistradoMensagem>
    {
        public FuncionarioRegistradoConsumidor(
            ILogger<FuncionarioRegistradoConsumidor> logger, 
            VendasContext contexto) : base(logger, contexto) { }

        public async Task Consume(ConsumeContext<FuncionarioVendasRegistradoMensagem> context)
        {
            var mensagem = context.Message;

            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var vendedor = new Vendedor(mensagem.Id, mensagem.Nome, mensagem.Email);

            await _contexto.Vendedores.AddAsync(vendedor);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}
