using PAC.Shared.Enums;

namespace PAC.RH.Models
{
    // Entidade (mais equivalente a uma Data Model que um Domain Model)
    public class Funcionario
    {
        public Guid Id { get; set; }
        public NomeCompleto NomeCompleto { get; set; }
        public DocumentoPessoal DocumentosPessoais { get; set; }
        public Email Email { get; set; }
        public Endereco Endereco { get; set; }
        public decimal SalarioBruto { get; set; }
        public Setor Setor { get; set; }
        public bool Desligado { get; set; }

        public Funcionario()
        {
            Id = Guid.NewGuid();
            Desligado = false;
        }

        public decimal CalcularSalarioLiquido()
        {
            throw new NotImplementedException();
        }
    }
}
