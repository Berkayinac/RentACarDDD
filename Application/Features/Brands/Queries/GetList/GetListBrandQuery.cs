using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Queries.GetList;

public class GetListBrandQuery : IRequest<GetListResponse<GetListBrandListItemDto>>, ICachableRequest, ILoggableRequest
{
    // Sayfalama bilgisi verilecek
    public PageRequest PageRequest { get; set; }

    // Unique bir isimlendirme oluşturabilmek için cache'lemeyi bu şekilde yaptık hangi sayfa sayfadaki veri sayısı şeklinde
    // Eğerki ama az markam var hepsini alıcam o zaman böyle pageındex'li vs bir dinamik cache oluşturmazsın direkt hepsini tek bir cache üzerinden işlemini yaparsın.

    public string CacheKey => $"GetListBrandQuery({PageRequest.PageIndex}, {PageRequest.PageSize})";

    public bool BypassCache { get; }

    public TimeSpan? SlidingExpiration { get; } // appsettings'den okunacak.

    // GetBrands isiminde CacheKey'lerin hepsini tutucaz ve yöneticez.
    public string? CacheGroupKey => "GetBrands";

    public class GetListBrandQueryHandler : IRequestHandler<GetListBrandQuery, GetListResponse<GetListBrandListItemDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetListBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListBrandListItemDto>> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
        {
            Paginate<Brand> brands = await _brandRepository.GetListAsync(
                 index: request.PageRequest.PageIndex,
                 size: request.PageRequest.PageSize,
                 cancellationToken: cancellationToken,
                 withDeleted: true // silinenlerin gelmesi için true olmalı gelmesini istemiyorsan false olarak kalacak.
                 );

            GetListResponse<GetListBrandListItemDto> response = _mapper.Map<GetListResponse<GetListBrandListItemDto>>(brands);
            return response;

        }
    }
}
