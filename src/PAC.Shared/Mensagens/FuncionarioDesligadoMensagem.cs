using PAC.Shared.Enums;

namespace PAC.Shared.Mensagens
{
    public record FuncionarioDesligadoMensagem(Guid Id, Setor Setor) : IntegracaoMensagem
    {
        public override string Topico => "funcionario.desligado";
    }
}
