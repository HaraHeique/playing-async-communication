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

        // Método ad-hock setter
        public void AtribuirNovoNome(NomeCompleto nome, Setor setorValido)
        {
            VerificarSetorValido(setorValido);

            NomeCompleto = nome;
        }
        
        // Método ad-hock setter
        public void AtribuirNovasInfoPessoais(string nome, string emailEmpresarial, Setor setorValido)
        {
            VerificarSetorValido(setorValido);

            // Como estou tratando ambos como value object e eles são imutáveis então estou instanciando novamente porque são imutáveis
            NomeCompleto = new NomeCompleto(nome, NomeCompleto.Apelido);
            Email = new Email(Email.Pessoal, emailEmpresarial);
        }

        private void VerificarSetorValido(Setor setorValido)
        {
            if (Setor != setorValido)
                throw new InvalidOperationException($"O setor {setorValido} é inválido para atualizar o nome");
        }
    }
}
