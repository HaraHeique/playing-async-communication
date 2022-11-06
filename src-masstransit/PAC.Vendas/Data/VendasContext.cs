using Microsoft.EntityFrameworkCore;
using PAC.Vendas.Models.Domain;

#nullable disable
namespace PAC.Vendas.Data
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

        public DbSet<Vendedor> Vendedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasContext).Assembly);
    }
}
