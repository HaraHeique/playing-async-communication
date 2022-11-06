using PAC.Shared.Enums;

namespace PAC.Shared.Mensagens
{
    public record FuncionarioAtualizadoMensagem(Guid Id, string Nome, Setor Setor)
        : IntegracaoMensagem
    {
        public override string Topico => "rh.funcionario.atualizado";
    }
}
