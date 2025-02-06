using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Services.Implementations;

public class ProductService(IGenericRepository<Product> productInterface, IMapper mapper) : IProductService
{
    public async Task<ServiceResponse> AddAsync(CreateProduct product)
    {
        var mappedData = mapper.Map<Product>(product);

        int result = await productInterface.AddAsync(mappedData);
        
        return result >0 ? new ServiceResponse(true, "Product Added!") : new ServiceResponse(false, "Unable to add the product") ;
        
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        bool result = await productInterface.DeleteAsync(id);

        if (result)
            return new ServiceResponse(result, "Product Deleted!");
        else
            return new ServiceResponse(result, "Unable to delete the product");
    }

    public async Task<IEnumerable<GetProduct>> GetAllAsync()
    {
        var result = await productInterface.GetAllAsync();
        if(!result.Any()) return [];

        return mapper.Map<IEnumerable<GetProduct>>(result);        
    }

    public async Task<GetProduct> GetByIdAsync(Guid id)
    {
        var result = await productInterface.GetByIdAsync(id);
        if (result == null) return new GetProduct();

        var mappedData = mapper.Map<GetProduct>(result);
        return mappedData;
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateProduct product)
    {
        var mappedData = mapper.Map<Product>(product);
        
        var result = await productInterface.UpdateAsync(mappedData);

        return result > 0 ? new ServiceResponse(true, "Product Updated!") : new ServiceResponse(false, "Unable to update the product");
    }
}
