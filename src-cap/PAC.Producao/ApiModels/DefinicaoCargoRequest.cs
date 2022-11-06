using PAC.Producao.Models;

namespace PAC.Producao.ApiModels
{
    public record DefinicaoCargoRequest(Guid Id, Cargo Funcao, Turno Periodo);
}
