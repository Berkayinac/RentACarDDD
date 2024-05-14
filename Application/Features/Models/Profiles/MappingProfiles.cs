using Application.Features.Models.Queries.GetList;
using Application.Features.Models.Queries.GetListByDynamic;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;

namespace Application.Features.Models.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // CreateMap ->> Brand -> CreateBrandCommand 'e default isimlendirmeler ile Map etmeyi sağlıyor.
        // ReverseMap() ise CreateBrandCommand'den -> Brand' e dönüştürmeyi sağlayan metot.

        CreateMap<Model, GetListModelListItemDto>()

            // Her bir c.BrandName 'e ait değeri -> opt.MapFrom(c => c.Brand.Name) değerinin içerisinden al'ı burada map ediyoruz manuel olarak
            // eğerki isim farklı vermek istersen bu yolu kullanabilirsin.
            // Veya kısaltmalı bir şekilde kullanım varsa bir yerlerde yine aynı şekilde burayı kullanabilirsin map etmek için.
            .ForMember(destinationMember: c => c.BrandName, memberOptions: opt => opt.MapFrom(c => c.Brand.Name))
            .ForMember(destinationMember: c => c.FuelName, memberOptions: opt => opt.MapFrom(c => c.Fuel.Name))
            .ForMember(destinationMember: c => c.TransmissionName, memberOptions: opt => opt.MapFrom(c => c.Transmission.Name))
            .ReverseMap();

        CreateMap<Model, GetListByDynamicModelListItemDto>()
            .ForMember(destinationMember: c => c.BrandName, memberOptions: opt => opt.MapFrom(c => c.Brand.Name))
            .ForMember(destinationMember: c => c.FuelName, memberOptions: opt => opt.MapFrom(c => c.Fuel.Name))
            .ForMember(destinationMember: c => c.TransmissionName, memberOptions: opt => opt.MapFrom(c => c.Transmission.Name))
            .ReverseMap();

        CreateMap<Paginate<Model>, GetListResponse<GetListModelListItemDto>>().ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemDto>>().ReverseMap();
    }
}