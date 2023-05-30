using AutoMapper;
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
                .Include(pelicula => pelicula.Generos)
                .Include(pelicula => pelicula.SalasDeCine)
                .ThenInclude(sala => sala.Cine)
                .Include(p =>p.PeliculasActores)
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
    }
}
