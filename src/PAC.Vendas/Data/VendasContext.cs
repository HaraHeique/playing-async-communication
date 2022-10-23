using Microsoft.EntityFrameworkCore;

namespace PAC.Vendas.Data
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options)
        {
        }
    }
}
