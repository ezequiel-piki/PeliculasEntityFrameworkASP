using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly AplicacionDbContext _context;
        private readonly IMapper _mapper;
        public PeliculasController(AplicacionDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get (int id)
        {
            var pelicula = await _context.Peliculas
                .Include(pelicula => pelicula.Generos.OrderByDescending(g => g.Nombre))
                .Include(pelicula => pelicula.SalasDeCine)
                .ThenInclude(sala => sala.Cine)
                .Include(p => p.PeliculasActores.Where( pa => pa.Actor.FechaNacimiento.Value.Year >= 1980 ))
                .ThenInclude(pa =>pa.Actor)
                .FirstOrDefaultAsync(pelicula => pelicula.Id == id);

           
            if(pelicula is null)
            {
                return NotFound();
            }
            var peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);
            peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();
            return peliculaDTO;

        }
        [HttpGet("conprojectto/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
        {
            var pelicula = await _context.Peliculas
                .ProjectTo<PeliculaDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pelicula => pelicula.Id == id);


            if (pelicula is null)
            {
                return NotFound();
            }

            pelicula.Cines = pelicula.Cines.DistinctBy(x => x.Id).ToList();
            return pelicula;

        }
        [HttpGet("cargadosSelectivo/{id:int}")]
        public async Task<ActionResult> GetSelectivo(int id)
        {
            var pelicula = await _context.Peliculas
            .Select(p => new
            {
                Id = p.Id,
                
                Titulo = p.Titulo,
                
                Generos = p.Generos
                .OrderByDescending(g => g.Nombre)
                .Select(g => g.Nombre)
                .ToList(),

                CantidadActores = p.PeliculasActores.Count(),
                CantidadCines = p.SalasDeCine.Select(S => S.CineId).Distinct().ToList()

            })
             .FirstOrDefaultAsync(p => p.Id == id);

            if(pelicula is null)
            {
                return NotFound();
            }
            return Ok(pelicula);
        }

        [HttpGet("cargadoExplicito/{id:int}")]
        public async Task <ActionResult<PeliculaDTO>> GetExplicito(int id)
        {
            var pelicula = await _context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
            // await _context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();

            var cantidadGeneros = await _context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

            if(pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);
            return peliculaDTO;
        }

        [HttpGet("lazyLoading/{id:int}")]
        public async Task<ActionResult<List<PeliculaDTO>>> GetLazyLoading(int id)
        {
            // var pelicula = await _context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id==id);
            var peliculas = await _context.Peliculas.AsTracking().ToListAsync();

            foreach (var pelicula in peliculas)
            {
                // cargar los generos de la pelicula

                //problema n+1
                pelicula.Generos.ToList();
            }

            //if(pelicula is null)
            //{
            //    return NotFound();  
            //}

            var peliculasDTOs = _mapper.Map<List<PeliculaDTO>>(peliculas);
           // peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(x => x.Id).ToList();

            return peliculasDTOs;
        }
    }
}
