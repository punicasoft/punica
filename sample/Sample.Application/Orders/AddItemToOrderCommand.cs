using Punica.Bp.CQRS.Messages;

namespace Sample.Application.Orders
{
    public class AddItemToOrderCommand : Item, ICommand
    {
        public Guid OrderId { get; set; }
    }
}
