namespace PAC.Producao.ApiModels
{
    public record AlteracaoNomeRequest(Guid Id, string Nome, string? Apelido, string Motivo);
}
