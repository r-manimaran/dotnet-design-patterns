using Application.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Domain.Specifications.OrderSpecifications;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<OrderResponseDto> CreateOrderAsync(CreatOrderDto creatOrderDto)
    {
        // create new Order and Order Item
        var order = new Order { OrderDate = creatOrderDto.OrderDate};


        foreach(var itemDto in creatOrderDto.Items)
        {
            
            var orderItem = new OrderItem 
            { 
                ProductId = itemDto.ProductId, 
                Quantity = itemDto.Quantity,
                Price = itemDto.Price
            };
            order.Items.Add(orderItem);
       
        }

        await _unitOfWork.GetRepository<Order>().AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return new OrderResponseDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            Items = order.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };
    }

    public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
    {
        
        var specification = new OrderWithItemsSpecification(id);
        var item = await _unitOfWork.GetRepository<Order>().GetBySpecificationAsync(specification);

        if (item == null)
            throw new Exception("Order Not found");


            return new OrderResponseDto
            {
                OrderId = item.Id,
                OrderDate = item.OrderDate,
                Items = item.Items.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        
    }

    //public async Task<IEnumerable<OrderResponseDto>> GetOrdersAsync()
    //{
    //    //var items = await _unitOfWork.GetRepository<Order>().GetAllAsync();
         
    //}
}
