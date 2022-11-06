namespace PAC.Vendas.Models.Domain
{
    public class Vendedor
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public Funcao? Funcao { get; set; }

        public Vendedor(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Funcao = null;
            Ativo = false;
        }

        public void AlterarNome(string nome) => Nome = nome;

        public void Desligar() => Ativo = false;

        public void DefinirCargo(Funcao funcao)
        {
            Funcao = funcao;
            Ativo = true;
        }

        public void AlterarInfoPessoais(string nome, string email, string motivo)
        {
            Nome = nome;
            Email = email;

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("Operação de alteração de info pessoais inválida");
        }
    }
}
