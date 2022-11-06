using MassTransit;
using PAC.Producao.Data;
using PAC.Shared.Mensagens;

namespace PAC.Producao.Consumidores
{
    public class FuncionarioDesligadoConsumidor : OperariosConsumidor, IConsumer<FuncionarioDesligadoMensagem>
    {
        public FuncionarioDesligadoConsumidor(
            ILogger<FuncionarioDesligadoConsumidor> logger,
            ProducaoContext producaoContext) : base(logger, producaoContext) { }

        public async Task Consume(ConsumeContext<FuncionarioDesligadoMensagem> context)
        {
            var mensagem = context.Message;

            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = await _contexto.Operarios.FindAsync(mensagem.Id);

            if (!OperarioExistente(operario, mensagem.Id)) return;

            operario!.Desligar();
            _contexto.Operarios.Update(operario);
            await _contexto.SaveChangesAsync();

            LogarMensagemProcessada(mensagem);
        }
    }
}
