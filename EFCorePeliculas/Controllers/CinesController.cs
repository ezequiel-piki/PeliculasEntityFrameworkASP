using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/cines")]
    public class CinesController : ControllerBase
    {
        private readonly AplicacionDbContext _context;
        private readonly IMapper _mapper;
        public CinesController(AplicacionDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet] 
        public async Task<IEnumerable<CineDTO>> Get()
        {
            return await _context.Cines.ProjectTo<CineDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("cercanos")]
        public async Task<IActionResult> Get(double latitud, double longitud)
        {
            var distanciaMaximaEnMetros = 2000;
            var geometryFactory =NtsGeometryServices.Instance.CreateGeometryFactory(srid:4326);
            var miUbicacion =geometryFactory.CreatePoint(new Coordinate(longitud,latitud));
            var cines = await _context.Cines
                .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
                .Where(c=>c.Ubicacion.IsWithinDistance(miUbicacion,distanciaMaximaEnMetros))
                .Select(c => new
                {
                    Nombre = c.Nombre,
                    Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion)),
                })
                .ToListAsync();
                
            return Ok(cines);
        }
    }
}
