using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;

namespace eCommerceApp.Host.Endpoints;

public static class AuthenticationEndpoints 
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Authentication")
                       .WithOpenApi()
                       .WithTags("Authentication");

        group.MapPost("/create", async (CreateUserRequest createUserRequest, 
                                            IAuthenticationService authenticationService) =>
        {
            var result = await authenticationService.CreateUser(createUserRequest);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithName("create");

        group.MapPost("/login", async (LoginRequest loginRequest, 
                                       IAuthenticationService authenticationService) =>
        {
            var result = await authenticationService.Login(loginRequest);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithName("login");

        group.MapGet("/refreshToken/{refreshToken}", async (string refreshToken, IAuthenticationService authenticationService) =>
        {
            var result = await authenticationService.ReviewToken(refreshToken);

            return result.Success? Results.Ok(result) : Results.BadRequest(result);

        }).WithName("renewToken");
    }
}
