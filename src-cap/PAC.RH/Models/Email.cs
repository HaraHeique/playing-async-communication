namespace PAC.RH.Models
{
    public class Email
    {
        public const int EnderecoTamanhoMinimo = 5;
        public const int EnderecoTamanhoMaximo = 254;

        public string Pessoal { get; set; }
        public string Empresarial { get; set; }

        public Email(string pessoal, string empresarial)
        {
            Pessoal = pessoal;
            Empresarial = empresarial;
        }
    }
}
