using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories;

// Hangi katman ile çalışılıyorsa o katmanda kullanılan tüm dış servislerin interfaceleri aynı katmanda olmak zorundadır.
// Bunun sebebi unit testler yazıldığında sadece application katmanı veya x bir katman test edildiği için başka bir katmana gidemeyiz.
// unit testleri yaparkende ilgili servis interface'lerimi mock'larım yani sahtelerini yaparım.



public interface ICarRepository : IAsyncRepository<Car, Guid>, IRepository<Car, Guid>
{

}
