using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Punica.Bp.CQRS;
using Sample.Application.Orders;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<ActionResult<Guid>> CreateOrder(CreateOrderRequest command)
        {
            var orderId = await _mediator.Send(command);

            return Ok(orderId);
        }

        [HttpPost("{orderId}")]
        public async Task<ActionResult<string>> AddItemToOrder(Guid orderId, AddItemToOrderCommand command)
        {
            command.OrderId = orderId;
            await _mediator.Send(command);

            return Ok("success");
        }
    }
}
