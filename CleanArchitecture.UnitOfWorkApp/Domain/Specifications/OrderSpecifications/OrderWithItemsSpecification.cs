using Domain.Common.Specificaitons;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications.OrderSpecifications;

public class OrderWithItemsSpecification : BaseSpecification<Order>
{
    public OrderWithItemsSpecification(int orderId)
        : base(o => o.Id == orderId)
    {
        AddInclude(o => o.Items);
    }
}
