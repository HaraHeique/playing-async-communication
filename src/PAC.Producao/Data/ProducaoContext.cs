using Microsoft.EntityFrameworkCore;

namespace PAC.Producao.Data
{
    public class ProducaoContext : DbContext
    {
        public ProducaoContext(DbContextOptions<ProducaoContext> options) : base(options)
        {
        }
    }
}
