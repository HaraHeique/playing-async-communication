using PAC.Shared.Enums;

namespace PAC.Shared.Mensagens
{
    public abstract record FuncionarioRegistradoMensagem(Guid Id, string Nome, Setor Setor)
        : IntegracaoMensagem
    {
        public override string Topico => "rh.funcionario.registrado";
    }

    public record FuncionarioProducaoRegistradoMensagem(Guid Id, string Nome, string? Apelido)
        : FuncionarioRegistradoMensagem(Id, Nome, Setor.Producao);

    public record FuncionarioVendasRegistradoMensagem(Guid Id, string Nome, string Email)
        : FuncionarioRegistradoMensagem(Id, Nome, Setor.Vendas);
}
