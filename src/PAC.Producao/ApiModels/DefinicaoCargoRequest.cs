using PAC.Producao.Models;

namespace PAC.Producao.ApiModels
{
    public class DefinicaoCargoRequest
    {
        public Guid Id { get; set; }
        public Cargo Funcao { get; set; }
        public Turno Periodo { get; set; }
    }
}
