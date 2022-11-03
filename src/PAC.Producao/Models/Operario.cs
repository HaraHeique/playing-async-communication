namespace PAC.Producao.Models
{
    public class Operario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string? Apelido { get; set; }
        public Cargo? Cargo { get; set; }
        public Turno? PeriodoTrabalho { get; set; }
        public bool Ativo { get; set; }

        public Operario(Guid id, string nome, string? apelido)
        {
            Id = id;
            Nome = nome;
            Apelido = apelido;
            Cargo = null;
            PeriodoTrabalho = null;
            Ativo = false;
        }

        public void DefinirCargo(Cargo funcao, Turno periodo)
        {
            Cargo = funcao;
            PeriodoTrabalho = periodo;
            Ativo = true;
        }
        
        public void AlterarNome(string nome, string? apelido, string motivo)
        {
            Nome = nome;
            Apelido = apelido;

            if (string.IsNullOrWhiteSpace(motivo))
                throw new InvalidOperationException("Operação inválida");
        }

        public void AlterarNome(string nome) => Nome = nome;
        
        public void Desligar() => Ativo = false;
    }
}
