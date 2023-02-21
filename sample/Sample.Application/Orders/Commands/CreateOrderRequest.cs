using Punica.Bp.CQRS.Messages;

namespace Sample.Application.Orders.Commands
{
    public class CreateOrderRequest : ICommand<Guid>
    {
        public string BuyerEmail { get; set; }
        public string BuyerName { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Units { get; set; }
    }
}
