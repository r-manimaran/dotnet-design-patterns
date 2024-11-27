using System;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Articles.CreateArticle;

public sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
{
    private readonly IArticleRepository _articleRepository;

    public CreateArticleCommandHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }
 
    public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article =  new Article
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            Tags = request.Tags,
            CreatedAt = DateTime.UtcNow,
        };

        var articleId = await _articleRepository.InsertAsync(article);

        return articleId;
    }
}
