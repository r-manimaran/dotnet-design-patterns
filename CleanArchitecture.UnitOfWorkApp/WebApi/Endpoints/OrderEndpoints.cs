using Application.Common.Models;
using Application.DTOs;
using Application.Services;
using Azure;
using Carter;
using Domain.Interfaces;

namespace WebApi.Endpoints;

public class OrderEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders/").WithOpenApi();

        group.MapGet("{id:int}", async (int id, IOrderService orderService) =>
        {
            try
            {
                var response = await orderService.GetOrderByIdAsync(id);
                return Results.Ok(ApiResponse<OrderResponseDto>.Succeed(response));

    }
            catch (Exception ex)
            {

                return Results.Ok(ApiResponse<OrderResponseDto>.Fail(ex.Message));
            };
        }).WithName("GetOrderById");

        //group.MapGet("all", async (int id, IOrderService orderService) =>
        //{
        //    var response = await orderService.GetOrdersAsync();
        //    return Results.Ok(response);
        //});
        group.MapPost("", async (CreatOrderDto request, IOrderService orderService) =>
        {
            try
            {
                var response = await orderService.CreateOrderAsync(request);
                return Results.Ok(ApiResponse<OrderResponseDto>.Succeed(response,
                    "Order Created Successfully"));
            }
            catch (Exception ex)
            {

                return Results.Ok(ApiResponse<OrderResponseDto>.Fail(ex.Message));
            }
        }).WithName("createOrder")
        .WithOpenApi();
    }
}
