using AutoMapper;
using DogApp.DTO;
using DogApp.Models;

namespace DogApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Person, PersonDTO>();
            CreateMap<PersonDTO, Person>();
            CreateMap<Dog, DogDTO>();
            CreateMap<DogDTO, Dog>();
        }
        
    }
}
