namespace PAC.Producao.ApiModels
{
    public class AlteracaoNomeRequest
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string? Apelido { get; set; }
        public string Motivo { get; set; }
    }
}
