namespace PAC.Shared.Mensagens
{
    public record VendedorInfoPessoaisAlteradaMensagem(Guid Id, string Nome, string Email)
        : IntegracaoMensagem
    {
        public override string Topico => "vendas.vendedor.alteracao-info-pessoais";
    }
}
