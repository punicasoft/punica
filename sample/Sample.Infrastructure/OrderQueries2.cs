using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Punica.Bp.EFCore.Querying;
using Sample.Application.Orders;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Infrastructure
{
    public class OrderQueries2 : QueryBase, IOrderQueries
    {
        public OrderQueries2(DbContext dbContext) : base(dbContext)
        {
        }


        public List<dynamic> GetPersons(List<string> columns, string filter)
        {
            return GetList<Order>(columns, filter);
        }

       
    }
}
