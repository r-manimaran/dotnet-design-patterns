namespace Application.DTOs;

public class OrderResponseDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> Items { get; set; }
}