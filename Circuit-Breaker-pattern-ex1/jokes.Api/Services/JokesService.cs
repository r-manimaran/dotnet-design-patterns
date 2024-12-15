using System;
using Polly.CircuitBreaker;

namespace jokes.Api.Services;
public class JokesService : IJokesService
{
    private const string JokeApiUrl = "https://official-joke-api.appspot.com/random_joke";
    private readonly HttpClient _httpClient;

    public JokesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetJokeAsync()
    {
        try
        {
            var request = new HttpRequestMessage
            (
                method: HttpMethod.Get,
                requestUri: JokeApiUrl
            );

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (BrokenCircuitException ex)
        {
            return $"The joke service is currently unavailable. Please try again later.{ex.Message}";
        }
        catch( HttpRequestException ex)
        {
            return $"An error occurred while fetching the joke. Please try again later.{ex.Message}";
        }
    }
}
