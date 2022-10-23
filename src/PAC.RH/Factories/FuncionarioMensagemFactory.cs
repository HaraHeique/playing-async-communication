using PAC.RH.Models;
using PAC.Shared.Enums;
using PAC.Shared.Mensagens;

namespace PAC.RH.Factories
{
    public static class FuncionarioMensagemFactory
    {
        public static FuncionarioRegistradoMensagem Criar(Funcionario funcionario)
        {
            if (funcionario.Setor == Setor.Producao)
                return new FuncionarioProducaoRegistradoMensagem(
                    funcionario.Id, funcionario.NomeCompleto.Nome, funcionario.NomeCompleto.Apelido
                );

            if (funcionario.Setor == Setor.Vendas)
                return new FuncionarioVendasRegistradoMensagem(
                    funcionario.Id, funcionario.NomeCompleto.Nome, funcionario.Email.Empresarial
                );

            throw new InvalidOperationException($"Não é possível criar a mensagem para o setor: {funcionario.Setor}");
        }
    }
}
