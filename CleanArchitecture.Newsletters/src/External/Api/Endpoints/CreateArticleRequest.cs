using System;

namespace Api.Endpoints;

public class CreateArticleRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<string> Tags { get; set; }
}