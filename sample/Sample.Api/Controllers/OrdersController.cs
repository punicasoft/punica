using Microsoft.AspNetCore.Mvc;
using Punica.Bp.CQRS;
using Sample.Application.Orders.Commands;
using Sample.Application.Orders.Queries;

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
        public async Task<ActionResult<Guid>> CreateOrder([FromHeader(Name = "TenantId")] Guid tenantId, CreateOrderRequest command)
        {
            var orderId = await _mediator.Send(command);

            return Ok(orderId);
        }

        [HttpPost("{orderId}")]
        public async Task<ActionResult<string>> AddItemToOrder(Guid orderId, [FromHeader(Name = "TenantId")]Guid tenantId, AddItemToOrderCommand command)
        {
            command.OrderId = orderId;
            await _mediator.Send(command);

            return Ok("success");
        }

        [HttpPost("details")]
        public async Task<ActionResult<string>> GetOrder(GetOrderQuery query , [FromHeader(Name = "TenantId")] Guid tenantId)
        {
            var order = await _mediator.Send(query);

            return Ok(order);
        }
    }
}
