using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PAC.Producao.Models;

namespace PAC.Producao.Data.TypesConfigurations
{
    public class OperarioTypeConfiguration : IEntityTypeConfiguration<Operario>
    {
        public void Configure(EntityTypeBuilder<Operario> builder)
        {
            builder.ToTable("Operarios", "Producao");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Nome)
                .IsRequired()
                .HasColumnType("VARCHAR(128)");

            builder.Property(o => o.Apelido)
                .IsRequired(false)
                .HasColumnType("VARCHAR(128)");

            builder.Property(o => o.PeriodoTrabalho)
                .IsRequired(false)
                .HasColumnType("TINYINT");

            builder.Property(o => o.Cargo)
                .IsRequired(false)
                .HasColumnType("TINYINT");
            
            builder.Property(o => o.Ativo)
                .IsRequired()
                .HasColumnType("BIT");
        }
    }
}
 