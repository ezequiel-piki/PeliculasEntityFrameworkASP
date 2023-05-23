using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : Controller
    {
        private readonly AplicacionDbContext _context;
        public GenerosController(AplicacionDbContext context) 
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genero>> Get()
        {
            return await _context.Generos.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> GetById(int id)
        {
            var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Identificador == id);
            if(genero is null) { 
             return NotFound();
            }
            return genero;
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Genero>> Primer()
        {
            var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Nombre.StartsWith("C"));
            
            if(genero is null)
            {
                return NotFound();
            }
            return genero;
        }
    }
}
