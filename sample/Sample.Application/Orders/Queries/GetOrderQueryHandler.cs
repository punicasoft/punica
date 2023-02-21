
using Punica.Bp.CQRS.Handlers;
using Punica.Bp.Ddd.Domain.Repository;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Queries
{
    public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, Order?>
    {
        private readonly IUnitOfWork _unitOfWork; //TEMPORARY USAGE

        public GetOrderQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Order>().FindAsync(query.OrderId, cancellationToken);
        }
    }
}
