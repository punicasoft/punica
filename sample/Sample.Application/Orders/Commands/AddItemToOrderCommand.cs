using Punica.Bp.CQRS.Messages;

namespace Sample.Application.Orders.Commands
{
    public class AddItemToOrderCommand : Item, ICommand
    {
        public Guid OrderId { get; set; }
    }
}
