using System;

namespace Github.Api;

public class GitHubSettings
{
    public const string SectionName = "GitHubSettings";
    public const string GitHubApiUrl = "https://api.github.com";
    public string AccessToken { get; set; } 
    public string UserAgent { get; set; }
}
