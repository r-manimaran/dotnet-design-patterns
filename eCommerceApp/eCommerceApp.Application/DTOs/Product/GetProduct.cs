using eCommerceApp.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.DTOs.Product;

public class GetProduct:Base
{
    public Guid Id { get; set; }
    public GetCategory? Category { get; set; } 
}
