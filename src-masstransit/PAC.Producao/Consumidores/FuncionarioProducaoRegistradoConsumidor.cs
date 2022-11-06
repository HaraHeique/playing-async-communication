using MassTransit;
using PAC.Producao.Data;
using PAC.Producao.Models;
using PAC.Shared.Mensagens;

namespace PAC.Producao.Consumidores
{
    public class FuncionarioProducaoRegistradoConsumidor : OperariosConsumidor, IConsumer<FuncionarioProducaoRegistradoMensagem>
    {
        public FuncionarioProducaoRegistradoConsumidor(
            ILogger<FuncionarioProducaoRegistradoConsumidor> logger,
            ProducaoContext producaoContext) : base(logger, producaoContext) { }

        public async Task Consume(ConsumeContext<FuncionarioProducaoRegistradoMensagem> context)
        {
            var mensagem = context.Message;

            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = new Operario(mensagem.Id, mensagem.Nome, mensagem.Apelido);

            await _contexto.Operarios.AddAsync(operario);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}

