using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_ComprasStock.Entidades;

namespace WebApi_ComprasStock.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        //....................................................................
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //......................................................................................
            base.OnModelCreating(modelBuilder);
            //......................................................................................
            modelBuilder.Entity<ProveedorProducto>().HasKey(x => new { x.ProveedorId, x.ProductoId });

            modelBuilder.Entity<DatosProveedores>()
                 .HasIndex(u => u.Cuit)
                 .IsUnique();
            modelBuilder.Entity<TipoProducto>()
                .HasIndex(p => p.CodigoTipo)
                .IsUnique();

            modelBuilder.Entity<Categorias>()
                .HasIndex(p => p.CodigoCategoria)
                .IsUnique();

            modelBuilder.Entity<Rubros>()
                .HasIndex(p => p.CodigoRubro)
                .IsUnique();

        }
        //....................................................................
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<DatosProveedores> DatosProveedores { get; set; }
        public DbSet<Diccionario_CIVA> Diccionario_CIVA { get; set; }
        public DbSet<TipoProducto> TipoProductos { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Rubros> Rubros { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProveedorProducto> ProveedorProducto { get; set; }
        public DbSet<UnidadDeMedida> UnidadDeMedida { get; set; }
    }
}
