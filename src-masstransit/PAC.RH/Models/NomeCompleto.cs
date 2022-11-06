namespace PAC.RH.Models
{
    // Value object
    public class NomeCompleto
    {
        public string Nome { get; set; }
        public string? Apelido { get; set; }

        // VALIDAÇÕES DOS VALUE OBJECTS (são sempre daquele jeito fixo e padronizado)

        public NomeCompleto(string nome, string? apelido)
        {
            Nome = nome;
            Apelido = apelido;
        }
    }
}
