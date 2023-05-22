using System.ComponentModel.DataAnnotations;

namespace EFCorePeliculas.Entidades
{
    public class Genero
    {
        public int Identificador { get; set; }

        // [StringLength(150)]
        //[MaxLength(150)]
       // [Required]
        public string Nombre { get; set;}
    }
}
