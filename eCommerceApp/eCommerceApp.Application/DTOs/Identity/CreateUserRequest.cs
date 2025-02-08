namespace eCommerceApp.Application.DTOs.Identity;

public class CreateUserRequest: Base
{
    public required string FullName { get; set; } 
    public required string ConfirmPassword { get; set; }
}
