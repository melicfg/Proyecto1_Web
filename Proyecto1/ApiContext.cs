using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;

namespace Proyecto1
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ParqueoDb");
        }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Factura> Facturas { get; set; }

        public DbSet<Parqueo> Parqueos { get; set; }

        public DbSet<Tiquete> Tiquetes { get; set; }


    }
}
