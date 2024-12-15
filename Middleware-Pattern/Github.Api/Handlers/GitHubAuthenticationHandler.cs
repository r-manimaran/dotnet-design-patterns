using System;
using Microsoft.Extensions.Options;

namespace Github.Api.Handlers;

public class GitHubAuthenticationHandler : DelegatingHandler
{
    private readonly IOptions<GitHubSettings> _options;

    public GitHubAuthenticationHandler(IOptions<GitHubSettings> options)
    {
        _options = options;
    }

   protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
                                                CancellationToken cancellationToken)
    {
        request.Headers.Add("Authorization", _options.Value.AccessToken);
        request.Headers.Add("User-Agent", _options.Value.UserAgent);

        return await base.SendAsync(request, cancellationToken);
    }
}
