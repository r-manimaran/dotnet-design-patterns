using System;

namespace Github.Api;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("api/users/{username}", async (
            string username,
            GitHubService githubService) =>
        {
            var content = await githubService.GetByUsernameAsync(username);
            return Results.Ok(content);
        });
    }
}
