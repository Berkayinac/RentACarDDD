using Application;
using Persistance;
using Core.CrossCuttingConcern.Exceptions.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration); // IConfiguration interface'ine ait concrete class'ý builder.Configuration kendi çözüyor.
builder.Services.AddHttpContextAccessor(); // Loglama için User'ýn bilgisine ihtiyaç duyuyoruz. HttpContextAccessor bize bu bilgiyi kullanmamýzý saðlýyor.

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration="localhost:6379"); // redis'in çalýþtýðý port.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// biz development ortamýnda çalýþýrken, hata bilgisini detaylý görmek isteriz ama kullanýcýya vermek istemediðimiz bir bilgi.
// bu sebepten bu middleware'ý sadece Production'da çalýþtýr.

//if (app.Environment.IsProduction())
    app.ConfigureCustomExceptionMiddleware();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
