
namespace jokes.Api.Services;

public interface IJokesService
{
    Task<string> GetJokeAsync();
}    
