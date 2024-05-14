using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Contexts;

public class BaseDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Fuel> Fuels { get; set; }
    public DbSet<Transmission> Transmissions { get; set; }
    public DbSet<Model> Models { get; set; }

    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }

    // DbContextOptions -> Veritabanına erişirken yapılan configurations
    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
        Database.EnsureCreated(); // önce veritabanının oluştuğundan emin ol.  -> Bu Kod Migration oluşturulurken yorum satırına alınmalı aksi taktirde tabloları bir kez daha oluşturmuş gibi davranıyor.
    }

    // Brand'ı olduğu gibi kullanmak istemiyoruz.
    // Kendi konfigurasyonlarımı yapmak istiyorum
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // mevcut assembly'deki tüm konfigurasyonları bul onları direkt uygula.
    }

}

// exception has been thrown by the target of an invocation
// add-migration init yaparken böyle bir hata alıyorum hocam çözümün bulamadım

// çözüm -> entityconfigrasyonda  model configrasyonunda cars karşılığı olmadığı için sorun çıkartıyordu
 