## Middleware Pattern For HttpClient With Delegating Handlers

```bash
> dotnet user-secrets init
```

```bash
> dotnet user-secrets set "GitHubSettings:AccessToken" "your-github-token"
> dotnet user-secrets set "GitHubSettings:UserAgent" "your-user-agent"
```

### Delegating Handlers

**Authentication Handler**
- Using this handler the Authentication Header information for GitHub api endpoint are passed.

**Logging Handler**
- Added a Logging Handler to log the information. Added before the Authentication Handler in the pipeline, so the flow will pass through this Logging Handler before the Authentication Handler.

**Retry Handler**
- Added a retry handler using Polly to retry (if failed) for specific number of times.
- To test the Retry handler, added a Randomness logic in Logging Handler.
- This handler is added on top of Logging Handler. 

So the flow will go by 
  RetryHandler ==> Logging Handler ==> Authentication Handler