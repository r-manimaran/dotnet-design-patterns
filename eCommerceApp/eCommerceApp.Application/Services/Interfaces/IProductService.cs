using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<GetProduct>> GetAllAsync();
    Task<GetProduct> GetByIdAsync(Guid id);
    Task<ServiceResponse> AddAsync(CreateProduct product);
    Task<ServiceResponse> UpdateAsync(UpdateProduct product);
    Task<ServiceResponse> DeleteAsync(Guid id);
}
