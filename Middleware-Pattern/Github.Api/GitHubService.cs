using System;

namespace Github.Api;

public class GitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<GitHubUser?> GetByUsernameAsync(string username)
    {
        var content = await _httpClient.GetFromJsonAsync<GitHubUser>($"users/{username}");
        return content;
    }
}
