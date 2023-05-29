using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;
using EFCorePeliculas.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class AutoresController : ControllerBase 
    {
        private readonly AplicacionDbContext _context;
        private readonly IMapper _mapper;
        public AutoresController( AplicacionDbContext context, IMapper mapper)
        {
            this._context = context;      
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            return await _context.Actores
               // .Select(actor => new ActorDTO {Id= actor.Id, Nombre = actor.Nombre})
                 .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
           
        }
    }
}
