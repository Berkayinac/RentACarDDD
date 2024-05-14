using Core.Application.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Core.Application.Pipelines.Validation;
using Core.Application.Pipelines.Transaction;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.CrossCuttingConcern.Serilog;
using Core.CrossCuttingConcern.Serilog.Logger;

namespace Application;

public static class ApplicationServiceRegistration
{
    //  Extension yazılabilmesi için Extension metodunun static olması gerekiyor
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper configurasyonunu yapmak için kullandık.
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // BaseBusinessRules -> inheritini yapan her class'ı bul bu projedeki ve onların kurallarını kullan.
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        // Validator'ları devreye sok.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(configuration =>
        {
            //  MediatR git bütün assembly'i tara command ve query'leri bul. onları handler'larını bul. Eşleş listele.
            // Controller içerisinde bir command send yaparsam onun handler'ını çalıştır.
            // Assembly.GetExecutingAssembly() -> Mevcut assembly'de ara diyoruz.
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // MediatR git Command veya Query çalıştıracaksan onlara ait olan bu middleware'den geçir.
            // bu middleware -> core içerisine gidecek ilgili command'in veya query'nin validator'u olup olmadığını kontrol edecek
            // eğer varsa onlara ait olan validator'ı çalıştırarak gelen request'teki verileri validate edecek.
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));

            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));

            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));

            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));

            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Hangi loglama sistemini kullanıcaz bunu belirtiyoruz. file mi, mssql mi neresi 
        //services.AddSingleton<LoggerServiceBase, FileLogger>();
        services.AddSingleton<LoggerServiceBase, MsSqlLogger>();

        return services;
    }

    // Alt sınıfı ne olarak verirsem ona ait olan tüm child class'ları IOC'ye ekle.
    // Business Rules'lar örnek olarak -> BaseBusinessRules olarak type'ı verdim bu type'ı hangi class'lar inherit alırsa
    // O class'ları IOC'ye ekle.
    public static IServiceCollection AddSubClassesOfType(
      this IServiceCollection services,
      Assembly assembly,
      Type type,
      Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
  )
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);

            else
                addWithLifeCycle(services, type);
        return services;
    }
}

// IOC container'lar için WebApi'ı kullanmak yerine her bir katman içerisinde ayrı bir Registration class'ı oluşturarak ilgili IOC durumunu buradan extension yardımı ile yapıyoruz.
// Buradaki yazılan Extension'ın çalışması için WebApi içerisindeki program.cs'ye giderek
// builder bölgesine -> builder.Services.AddApplicationServices(); yazmamız bu extension'ı devreye koyacaktır.