namespace Products.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } =string.Empty;
    public string Description { get; set; } =string.Empty;
    public Brand Brand { get; set; }
    public int BrandId { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
}
