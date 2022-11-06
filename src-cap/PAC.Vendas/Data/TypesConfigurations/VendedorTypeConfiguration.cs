using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PAC.Vendas.Models.Domain;

namespace PAC.Vendas.Data.TypesConfigurations
{
    public class VendedorTypeConfiguration : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedores", "Vendas");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .IsRequired()
                .HasColumnName("Id");

            builder.Property(v => v.Nome)
                .IsRequired()
                .HasColumnType("VARCHAR(128)");
            
            builder.Property(v => v.Email)
                .IsRequired()
                .HasColumnType("VARCHAR(256)");

            builder.Property(v => v.Funcao)
                .IsRequired(false)
                .HasColumnType("SMALLINT");

            builder.Property(v => v.Ativo)
                .IsRequired()
                .HasColumnType("BIT");
        }
    }
}
