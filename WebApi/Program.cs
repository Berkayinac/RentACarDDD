using Application;
using Persistance;
using Core.CrossCuttingConcern.Exceptions.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration); // IConfiguration interface'ine ait concrete class'� builder.Configuration kendi ��z�yor.
builder.Services.AddHttpContextAccessor(); // Loglama i�in User'�n bilgisine ihtiya� duyuyoruz. HttpContextAccessor bize bu bilgiyi kullanmam�z� sa�l�yor.

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(opt => opt.Configuration="localhost:6379"); // redis'in �al��t��� port.

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

// biz development ortam�nda �al���rken, hata bilgisini detayl� g�rmek isteriz ama kullan�c�ya vermek istemedi�imiz bir bilgi.
// bu sebepten bu middleware'� sadece Production'da �al��t�r.

//if (app.Environment.IsProduction())
    app.ConfigureCustomExceptionMiddleware();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
