using AutoMapper;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProduct, Product>();
        CreateMap<UpdateProduct, Product>();
        CreateMap<Product, GetProduct>();
    }
}
