using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Queries.GetById;

public class GetByIdBrandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}

// GET Response'larında her bir metot için farklı response kullanmamızın sebebi
// List işlemlerinde sadece kritik veriler gözükmesini isterken
// Detaya inildiğinde yani tek bir ID'ye ait verinin detayına inildiğinde
// Veriye ait tüm detayları görebilelim diye her bir metot için farklı response'lar oluşturuyoruz.

// örnek olarak e-ticaret sitesindeki ürünler listelendiğinde ismi, fiyatı, markası vs varken
// detaya inildiğinde ürüne ait yorumlar, değerlendirmeler, ayrıntılı bilgiler, kaç kişi satın almış gibi gibi detay bilgilere ulaşabiliyoruz.