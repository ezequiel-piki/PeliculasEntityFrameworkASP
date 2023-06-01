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
            return await _context.Generos.OrderBy(genero => genero.Nombre).ToListAsync();
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
        [HttpGet("filtrar")]
        public async Task <IEnumerable<Genero>> Filtrar(string nombre)
        {
            return await _context.Generos
                .Where(genero => genero.Nombre.Contains(nombre))
                //.OrderBy(genero => genero.Nombre)
                //.OrderByDescending(genero => genero.Nombre)
                .ToListAsync();
        }

        [HttpGet("paginacion")]
        public async Task<IEnumerable<Genero>> GetPaginacion(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 2;

            var generos = await _context.Generos
                .Skip((pagina-1) * cantidadRegistrosPorPagina)
                .Take(2)
                .ToListAsync();
            return generos;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Genero genero)
        {
            var status1 = _context.Entry(genero).State;
            _context.Generos.Add(genero);
            var status2 = _context.Entry(genero).State;
            await _context.SaveChangesAsync();
            var status3 = _context.Entry(genero).State;
            return Ok();
        }

        [HttpPost("varios")]
        public async Task<ActionResult> Post(Genero[] generos)
        {
            _context.AddRange(generos);
            await _context.SaveChangesAsync();
            return Ok();    
        }
    }
}
