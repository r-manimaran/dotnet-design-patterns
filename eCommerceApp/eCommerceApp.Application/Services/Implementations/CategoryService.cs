using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Services.Implementations;

public class CategoryService(IGenericRepository<Category> categoryInterface, IMapper mapper) : ICategoryService
{
    public async Task<ServiceResponse> AddAsync(CreateCategory category)
    {
        var mappedData = mapper.Map<Category>(category);

        var result = await categoryInterface.AddAsync(mappedData);
        
        return result > 0 ? new ServiceResponse(true,"Category added successfully") : new ServiceResponse(false,"Category not added");
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        var result = await categoryInterface.DeleteAsync(id);

        return result  ? new ServiceResponse(true, "Category deleted successfully") : new ServiceResponse(false, "Category not deleted");
    }

    public async Task<IEnumerable<GetCategory>> GetAllAsync()
    {
        var result = await categoryInterface.GetAllAsync();

        if(!result.Any()) return [];

        return mapper.Map<IEnumerable<GetCategory>>(result);
    }

    public async Task<GetCategory> GetByIdAsync(Guid id)
    {
        var result = await categoryInterface.GetByIdAsync(id);

        if(result == null) return new GetCategory();

        return mapper.Map<GetCategory>(result);
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateCategory product)
    {
        var mappedData = mapper.Map<Category>(product);
        var result = await categoryInterface.UpdateAsync(mappedData);
        return result > 0 ? new ServiceResponse(true, "Category updated successfully") : new ServiceResponse(false, "Category not updated");
    }
}
