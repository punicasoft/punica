namespace Sample.Domain.Aggregates.Orders
{
    public record Buyer
    {
        public string Email { get; init; }
        public string Name { get; init; }
    }
}
