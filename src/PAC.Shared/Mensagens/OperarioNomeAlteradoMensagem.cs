namespace PAC.Shared.Mensagens
{
    public record OperarioNomeAlteradoMensagem(Guid Id, string Nome, string? Apelido) 
        : IntegracaoMensagem
    {
        public override string Topico => "operario.nome-alterado";
    }
}
