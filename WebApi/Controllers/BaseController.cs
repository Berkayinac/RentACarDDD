using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class BaseController : ControllerBase
{
    private IMediator? _mediator;

    // Mediator => _mediator?? buradaki mediator set edilmiş mi ? -> bunu anlamak için "??" ifadesini kullanıyoruz. varsa bu _mediator ifadesini return edecektir.
    // yoksa -> ??= yazılarak HttpContext.RequestServices.GetService<IMediator>(); komutunu kullanarak IOC üzerinden gerekli olan class'ın referansını al
    // eğerki ??= yazmayıp ?? = gibi bir yapı kullanırsan hata alacaksın.
    protected IMediator? Mediator => _mediator??= HttpContext.RequestServices.GetService<IMediator>();
}
