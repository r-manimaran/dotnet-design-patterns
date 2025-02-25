using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CreatOrderDto
{
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
