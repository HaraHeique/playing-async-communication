namespace PAC.Shared.Mensagens
{
    public abstract record IntegracaoMensagem
    {
        public DateTime OcorrenciaEm { get; } = DateTime.Now;
        public abstract string Topico { get; }
    }
}
