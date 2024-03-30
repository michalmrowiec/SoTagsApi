using AutoMapper;
using SoTagsApi.Application.Tags.Queries;
using SoTagsApi.Domain.Models;

namespace SoTagsApi.Application.Mappings
{
    public class MapingProfile : Profile
    {
        public MapingProfile()
        {
            CreateMap<Tag, TagDto>();

            CreateMap<PagedResult<Tag>, PagedResult<TagDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
