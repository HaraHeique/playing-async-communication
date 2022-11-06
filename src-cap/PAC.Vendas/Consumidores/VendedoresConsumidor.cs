using DotNetCore.CAP;
using Newtonsoft.Json;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;
using PAC.Vendas.Data;
using PAC.Vendas.Models.Domain;

namespace PAC.Vendas.Consumidores
{
    public class VendedoresConsumidor : ICapSubscribe
    {
        private readonly ILogger<VendedoresConsumidor> _logger;
        private readonly VendasContext _contexto;

        public VendedoresConsumidor(ILogger<VendedoresConsumidor> logger, VendasContext contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [CapSubscribe("rh.funcionario.registrado")]
        public async Task Consumir(FuncionarioVendasRegistradoMensagem mensagem, CancellationToken cancellationToken)
        {
            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var vendedor = new Vendedor(mensagem.Id, mensagem.Nome, mensagem.Email);

            await _contexto.Vendedores.AddAsync(vendedor, cancellationToken);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }

        [CapSubscribe("rh.funcionario.atualizado")]
        public async Task Consumir(FuncionarioAtualizadoMensagem mensagem, CancellationToken cancellationToken)
        {
            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var vendedor = await _contexto.Vendedores.FindAsync(mensagem.Id);

            if (!VendedorExistente(vendedor, mensagem.Id)) return;

            vendedor!.AlterarNome(mensagem.Nome);
            _contexto.Vendedores.Update(vendedor);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }

        [CapSubscribe("rh.funcionario.desligado")]
        public async Task Consumir(FuncionarioDesligadoMensagem mensagem, CancellationToken cancellationToken)
        {
            if (SetorInvalido(mensagem.Setor)) return;

            LogarMensagemConsumida(mensagem);

            // Realizar validações na mensagem se desejado

            var operario = await _contexto.Vendedores.FindAsync(mensagem.Id);

            if (!VendedorExistente(operario, mensagem.Id)) return;

            operario!.Desligar();
            _contexto.Vendedores.Update(operario);
            await _contexto.SaveChangesAsync(cancellationToken);

            LogarMensagemProcessada(mensagem);
        }

        private void LogarMensagemProcessada(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem {@tipo} processada com sucesso", mensagem.GetType().Name);

        private void LogarMensagemConsumida(IntegracaoMensagem mensagem)
            => _logger.LogInformation("Mensagem consumida - {@tipo}: {@mensagem}", mensagem.GetType().Name, JsonConvert.SerializeObject(mensagem));

        private static bool SetorInvalido(Setor setor)
            => setor != Setor.Vendas;

        private bool VendedorExistente(Vendedor? vendedor, Guid identificador)
        {
            if (vendedor is null)
            {
                _logger.LogError("Vendedor com Id {@id} não encontrado na base de dados", identificador);
                return false;
            }

            return true;
        }
    }
}
