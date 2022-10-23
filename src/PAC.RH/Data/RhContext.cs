using Microsoft.EntityFrameworkCore;
using PAC.RH.Models;

#nullable disable
namespace PAC.RH.Data
{
    public class RhContext : DbContext
    {
        public RhContext(DbContextOptions<RhContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RhContext).Assembly);
        }
    }
}
