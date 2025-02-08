using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;

namespace eCommerceApp.Application.Services.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<ServiceResponse> CreateUser(CreateUserRequest user);
    Task<LoginResponse> Login(LoginRequest user);
    Task<LoginResponse> ReviewToken(string refreshToken);
}
