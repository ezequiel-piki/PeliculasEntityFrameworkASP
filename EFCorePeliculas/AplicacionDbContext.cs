using EFCorePeliculas.Entidades;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext (DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Genero>().HasKey(prop => prop.Identificador);
            modelBuilder.Entity<Genero>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();
        }
        public DbSet<Genero> Generos { get; set; }
    }
}
