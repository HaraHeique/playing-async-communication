using Microsoft.EntityFrameworkCore;
using PAC.Producao.Models;

#nullable disable
namespace PAC.Producao.Data
{
    public class ProducaoContext : DbContext
    {
        public ProducaoContext(DbContextOptions<ProducaoContext> options) : base(options) { }

        public DbSet<Operario> Operarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProducaoContext).Assembly);
        }
    }
}
