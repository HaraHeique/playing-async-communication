using System.Text.Json.Serialization;

namespace PAC.Shared.Mensagens
{
    public abstract record IntegracaoMensagem
    {
        [JsonInclude]
        public DateTime OcorrenciaEm { get; } = DateTime.Now;

        [JsonInclude]
        public abstract string Topico { get; }
    }
}
