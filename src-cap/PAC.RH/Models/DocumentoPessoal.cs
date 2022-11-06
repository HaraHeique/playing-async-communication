namespace PAC.RH.Models
{
    // Value Object (Poderia separar Cpf e Rg em dois value objects separados)
    public class DocumentoPessoal
    {
        public const int TamanhoCpf = 11;
        public const int TamanhoRg = 9;

        public string Cpf { get; set; }
        public string Rg { get; set; }

        // VALIDAÇÕES DOS VALUE OBJECTS (são sempre daquele jeito fixo e padronizado)
    }
}
