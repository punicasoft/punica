using Sample.Domain.Aggregates.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Orders
{
    public interface IOrderQueries
    {
        List<dynamic> GetPersons(string columns, string filter);
    }
}
