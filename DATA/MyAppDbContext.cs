using INVENTARIOZAP.Models;
using Microsoft.EntityFrameworkCore;

namespace INVENTARIOZAP.DATA
{
    public class MyAppDbContext : DbContext
    {
        public DbSet<ProductoModel> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Cambia la cadena de conexión si es necesario
            optionsBuilder.UseMySql("server=localhost;database=zapateria;user=root;password=;",
                new MySqlServerVersion(new Version(8, 0, 34)));
        }
    }
}
