using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerceApp.Application.DTOs.Category;

namespace eCommerceApp.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<GetCategory>> GetAllAsync();
    Task<GetCategory> GetByIdAsync(Guid id);
    Task<ServiceResponse> AddAsync(CreateCategory category);
    Task<ServiceResponse> UpdateAsync(UpdateCategory category);
    Task<ServiceResponse> DeleteAsync(Guid id);
}
