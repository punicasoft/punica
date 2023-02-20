﻿using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.EFCore;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override IQueryable<Order> IncludeAll(IQueryable<Order> query)
        {
           return query.Include(o => o.Items);
        }
    }
}
