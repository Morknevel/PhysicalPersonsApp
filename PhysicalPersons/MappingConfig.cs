using AutoMapper;
using Dal.Enums;
using Dal.Models;
using Dal.Models.DTO;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.UpdateDTO;

namespace PhysicalPersons;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<PersonCreateDto, Person>()
            .ForMember(dest => dest.PhoneNumbers,
                opt => opt.MapFrom(src => src.PhoneNumbers));
        CreateMap<PersonUpdateDto, Person>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CityUpdateDto))
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers));
        CreateMap<CityUpdateDto, City>();
        CreateMap<PhoneNumberUpdateDto, PhoneNumber>().ReverseMap();
        CreateMap<PhoneNumberDto, PhoneNumber>().ReverseMap();
        CreateMap<RelationshipCreateDto, Relationship>().ReverseMap();
        CreateMap<Person, PersonDto>()
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.RelatedTo,
                opt => opt.MapFrom(src => src.RelatedTo.Select(r => new RelationshipDto
                {
                    PersonId = src.PersonId,
                    RelatedId = r.RelatedId,
                    RelationType = r.RelationType
                })))
            .ForMember(dest => dest.RelatedFrom,
                opt => opt.MapFrom(src => src.RelatedFrom.Select(r => new RelationshipDto
                {
                    PersonId = r.PersonId,
                    RelatedId = src.PersonId,
                    RelationType = r.RelationType
                })));

        CreateMap<CityCreateDto, City>().ReverseMap();
        CreateMap<City, CityDto>().ReverseMap();


        CreateMap<PhoneNumberCreateDto, PhoneNumber>();
    }
}