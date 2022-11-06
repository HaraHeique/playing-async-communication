namespace PAC.Vendas.Models.Api
{
    public record AlteracaoInformacoesPessoaisRequest(Guid Id, string Nome, string Email, string Motivo);
}
