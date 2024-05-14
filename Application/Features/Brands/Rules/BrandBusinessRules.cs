using Application.Features.Brands.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcern.Exceptions.Types;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Rules;

public class BrandBusinessRules : BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository;

    public BrandBusinessRules(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task BrandNameCannotBeDuplicatedWhenInserted(string name)
    {
        Brand? result = await _brandRepository.GetASync(predicate: b => b.Name.ToLower() == name.ToLower());

        if (result != null)
        {
            // eğer ki burada hata alırsak yani bir BusinessException çıkarsa global hata yönetimi gerçekleştirilir.
            throw new BusinessException(BrandsMessages.BrandNameExists);
        }
    }
}
