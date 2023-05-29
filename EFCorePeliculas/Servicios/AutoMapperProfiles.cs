using AutoMapper;
using EFCorePeliculas.DTOs;
using EFCorePeliculas.Entidades;


namespace EFCorePeliculas.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<Actor,ActorDTO>();
            CreateMap<Cine, CineDTO>()
                .ForMember(dto => dto.Latitud, entidad => entidad.MapFrom(prop => prop.Ubicacion.Y))
                .ForMember(dto => dto.Longitud, entidad => entidad.MapFrom(prop => prop.Ubicacion.X));
        }
    }
}
