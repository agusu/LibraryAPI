using AutoMapper;
using Domain.Entities;
using Application.DTOs;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<Book, BookDto>()
            .ForMember(d => d.AuthorName,
                opt => opt.MapFrom(s => s.Author != null ? s.Author.Name : string.Empty));
    }
}