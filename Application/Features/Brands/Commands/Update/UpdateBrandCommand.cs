using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.Update;

public class UpdateBrandCommand : IRequest<UpdatedBrandResponse>, ICacheRemoverRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string CacheKey => "";

    public bool BypassCache => false;

    public string? CacheGroupKey => "GetBrands";



    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, UpdatedBrandResponse>
    {
        IBrandRepository _brandRepository;
        IMapper _mapper;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<UpdatedBrandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetASync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);

            brand = _mapper.Map(request, brand); // Bu formatta yazdığımızda -> Request'teki bilgiyi brand nesnesine taşıyacaktır.

            await _brandRepository.UpdateAsync(brand);

            UpdatedBrandResponse response = _mapper.Map<UpdatedBrandResponse>(brand);
            return response;
        }
    }
}