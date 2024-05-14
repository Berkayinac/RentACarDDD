using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.Create;

// ÖNEMLİİİ !!!
// Bir command veya query içerisinden başka bir Command veya query çağırılamaz.
// onun yerine service oluşturulup o service ayni manager class'ları aracılığı ile metotlar çağrılır.

// Örnek olarak Brand içerisinde başka bir class'a ait metot lazım oldu gidip o class'ın service'ini injection edip o şekilde o metoda ulaşılması gerekilir.


//Commands -> Insert, Update, Delete komutlarını kullanacağımız yerdir.Yani veritabanında değişiklik yaptığımız yerdir.

public class CreateBrandCommand : IRequest<CreatedBrandResponse>, ITransactionalRequest, ICacheRemoverRequest, ILoggableRequest
{
    public string Name { get; set; }

    // Sistemde herhangi bir Insert, Update, Delete işlemi olduğunda İlgili class (Brand ) ile ilgili tüm nesneleri cache'den uçurmalıyım.
    // eğerki sistemde tek sabit bir anahtar verilmiştir CacheKey property'sine onu set ederek tüm Brandleri kaldırırsın ama bizim sistemimiz dinamik bir yapıda olduğu için
    // içerisinde brand geçen tüm cache'leri gruplandırarak ortadan kaldırılmasını sağlarsın.
    public string CacheKey => "";

    public bool BypassCache => false;

    public string? CacheGroupKey => "GetBrands";

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {

        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinessRules;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandBusinessRules = brandBusinessRules;
        }
         
        public async Task<CreatedBrandResponse>? Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);

            Brand brand = _mapper.Map<Brand>(request); // gelen request'i brand'e maple
            brand.Id = Guid.NewGuid();

            //Brand brand2 = _mapper.Map<Brand>(request); // transaction görülmesi için comment'lendi.
            //brand2.Id = Guid.NewGuid(); // transaction görülmesi için comment'lendi.

            await _brandRepository.AddAsync(brand);
            //await _brandRepository.AddAsync(brand2); // transaction görülmesi için comment'lendi.

            CreatedBrandResponse createdBrandResponse = _mapper.Map<CreatedBrandResponse>(brand);
            return createdBrandResponse;
        }
    }
}
