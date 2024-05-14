using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.EntityConfigurations;

// IEntityTypeConfiguration -> implementasyonunu kullanan tüm class'ları BaseDbContext içerisindeki OnModelCreating metodu içerisindeki ApplyConfigurationsFromAssembly metodu bulacak ve konfigrasyon için kullanacak.
public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        // Hangi tabloya karşılık gelecek bu configuration && HasKey vererek -> Primary key'i belirtiyoruz Id olarak.
        builder.ToTable("Brands").HasKey(b=>b.Id); 

        builder.Property(b => b.Id).HasColumnName("Id").IsRequired();
        builder.Property(b => b.Name).HasColumnName("Name").IsRequired();
        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        // Unique yapılar olan marka adları tekrar edemesin. Bunu sağlayan yani kod tekrarını engelleyen metot.
        builder.HasIndex(b => b.Name, name:"UK_Brands_Name").IsUnique();

        builder.HasMany(b=>b.Models);

        // Her yapılan sorguya mutlaka eklemesini gereken yapıları bu metoda yazıyoruz.
        // Default olarak soft delete ile silinen verileri getirmesini istemiyoruz.
        // Bunu sağlayan yapı GLOBAL QUERY FILTER metodu olan HasQueryFilter() metodu.
        // DeletedDate.HasValue -> value su boş ise "!" var b'nin başında dikkat.
        builder.HasQueryFilter(b=>!b.DeletedDate.HasValue);
    }
}