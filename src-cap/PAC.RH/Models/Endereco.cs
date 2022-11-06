namespace PAC.RH.Models
{
    // Value object
    public class Endereco
    {
        public int? Numero { get; set; }
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        // VALIDAÇÕES DOS VALUE OBJECTS (são sempre daquele jeito fixo e padronizado)
    }
}
