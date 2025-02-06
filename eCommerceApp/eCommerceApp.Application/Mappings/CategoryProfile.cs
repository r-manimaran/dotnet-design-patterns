using AutoMapper;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategory, Category>();
        CreateMap<Category, GetCategory>();
    }
}
