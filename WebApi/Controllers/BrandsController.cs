using Application.Features.Brands.Commands.Create;
using Application.Features.Brands.Commands.Delete;
using Application.Features.Brands.Commands.Update;
using Application.Features.Brands.Queries.GetById;
using Application.Features.Brands.Queries.GetList;
using Core.Application.Requests;
using Core.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand)
        {
            CreatedBrandResponse response = await Mediator.Send(createBrandCommand);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListBrandQuery getListBrandQuery = new() { PageRequest = pageRequest };
            GetListResponse<GetListBrandListItemDto> response = await Mediator.Send(getListBrandQuery);
            return Ok(response);
        }

        // 1- Eğerki [HttpGet] olarak bırakırsak veriyi () içerisinde herhangi bir bilgi vermediğimiz için bir ambiguous yani belirsizlik durumu ortaya çıkacaktır.
        // isimlendirme için Route'tan gelen id'yi kullandıkki ayırabilsin.
        // 2- FromRoute 'dan veri okuduğumuz için {id}'yi route ederek erişebiliriz.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetByIdBrandQuery getByIdBrandQuery = new() { Id = id };
            GetByIdBrandResponse response = await Mediator.Send(getByIdBrandQuery);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBrandCommand updateBrandCommand)
        {
            UpdatedBrandResponse response = await Mediator.Send(updateBrandCommand);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            DeletedBrandResponse response = await Mediator.Send(new DeleteBrandCommand { Id = id });

            return Ok(response);
        }
    }
}
