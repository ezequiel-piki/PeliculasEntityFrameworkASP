using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.Configuraciones;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas
{
    public class AplicacionDbContext : DbContext
    {
        public AplicacionDbContext (DbContextOptions options) : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GeneroConfig());

            modelBuilder.ApplyConfiguration(new ActorConfig());

           
            
            modelBuilder.Entity<Cine>().Property(prop => prop.Nombre).HasMaxLength(150).IsRequired();
          
            modelBuilder.Entity<Pelicula>().Property(prop => prop.Titulo).HasMaxLength(250).IsRequired();
           
            modelBuilder.Entity<Pelicula>().Property(prop => prop.PosterUrl).HasMaxLength(500).IsUnicode(false);

            modelBuilder.Entity<CineOferta>().Property(prop => prop.PorcentajeDescuento).HasPrecision(precision: 5, scale: 2);
           

            modelBuilder.Entity<SalaDeCine>().Property(prop => prop.Precio).HasPrecision(precision:9,scale:2);
            modelBuilder.Entity<SalaDeCine>().Property(prop => prop.TipoSalaDeCine).HasDefaultValue(TipoSalaDeCine.DosDimensiones);

            modelBuilder.Entity<PeliculaActor>()
                .HasKey(prop => new { prop.PeliculaId,prop.ActorId });

            modelBuilder.Entity<PeliculaActor>().Property(prop => prop.Personaje).HasMaxLength(150);
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula>Peliculas { get; set; }
        public DbSet<CineOferta>CinesOfertas { get; set; }
        public DbSet<SalaDeCine>SalasDeCine { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
    }
}
