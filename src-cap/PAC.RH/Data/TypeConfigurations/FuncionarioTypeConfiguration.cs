using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PAC.RH.Models;

namespace PAC.RH.Data.TypeConfigurations
{
    public class FuncionarioTypeConfiguration : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.ToTable("Funcionarios", "Rh");

            builder.HasKey(f => f.Id);

            builder.OwnsOne(f => f.NomeCompleto, ownerBuilder =>
            {
                ownerBuilder.Property(n => n.Nome)
                    .IsRequired()
                    .HasColumnName("Nome")
                    .HasColumnType("VARCHAR(200)");

                ownerBuilder.Property(n => n.Apelido)
                    .IsRequired(false)
                    .HasColumnName("Apelido")
                    .HasColumnType("VARCHAR(200)");
            });

            builder.OwnsOne(f => f.DocumentosPessoais, ownerBuilder =>
            {
                ownerBuilder.Property(d => d.Cpf)
                    .IsRequired()
                    .HasColumnName("Cpf")
                    .HasColumnType($"VARCHAR({DocumentoPessoal.TamanhoCpf})");

                ownerBuilder.Property(d => d.Rg)
                    .IsRequired()
                    .HasColumnName("Rg")
                    .HasColumnType($"VARCHAR({DocumentoPessoal.TamanhoRg})");
            });

            builder.OwnsOne(f => f.Email, ownerBuilder =>
            {
                ownerBuilder.Property(e => e.Pessoal)
                    .IsRequired()
                    .HasColumnName("EmailPessoal")
                    .HasColumnType($"VARCHAR({Email.EnderecoTamanhoMaximo})");

                ownerBuilder.Property(d => d.Empresarial)
                    .IsRequired()
                    .HasColumnName("EmailEmpresarial")
                    .HasColumnType($"VARCHAR({Email.EnderecoTamanhoMaximo})");
            });

            builder.OwnsOne(f => f.Endereco, ownerBuilder =>
            {
                ownerBuilder.Property(e => e.Numero)
                    .IsRequired(false)
                    .HasColumnName("Numero")
                    .HasColumnType("INT");

                ownerBuilder.Property(e => e.Rua)
                    .IsRequired()
                    .HasColumnName("Rua")
                    .HasColumnType("VARCHAR(128)");

                ownerBuilder.Property(e => e.Complemento)
                    .IsRequired(false)
                    .HasColumnName("Complemento")
                    .HasColumnType("VARCHAR(250)");

                ownerBuilder.Property(e => e.Bairro)
                    .IsRequired()
                    .HasColumnName("Bairro")
                    .HasColumnType("VARCHAR(128)");

                ownerBuilder.Property(e => e.Cep)
                    .IsRequired()
                    .HasColumnName("Cep")
                    .HasColumnType("VARCHAR(8)");

                ownerBuilder.Property(e => e.Cidade)
                    .IsRequired()
                    .HasColumnName("Cidade")
                    .HasColumnType("VARCHAR(128)");

                ownerBuilder.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("Estado")
                    .HasColumnType("VARCHAR(128)");
            });

            builder.Property(f => f.SalarioBruto)
                .IsRequired()
                .HasColumnName("SalarioBruto")
                .HasColumnType("DECIMAL(18,2)");

            builder.Property(f => f.Setor)
                .IsRequired()
                .HasColumnName("Setor")
                .HasColumnType("INT");

            builder.Property(f => f.Desligado)
                .IsRequired()
                .HasColumnType("BIT");
        }
    }
}
