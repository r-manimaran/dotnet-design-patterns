using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public interface IOrderService 
{
    Task<OrderResponseDto> CreateOrderAsync(CreatOrderDto creatOrderDto);
    //Task<IEnumerable<OrderResponseDto>> GetOrdersAsync();

    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
}
