using System;
using Domain.Repositories;
using MediatR;

namespace Application.Articles.PublishArticle;

public class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand>
{
    private readonly IArticleRepository _articleRepository;

    public PublishArticleCommandHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;    
    }

    public async Task Handle(PublishArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.Id);

        if (article is null)
        {
            //throw new ArticleNotFound("Article not found.");
        }
        if (article.PublishedAt is not null)
        {
            //throw new ArticleAlreadyPublishedException("Article already published.");
        }

        article.PublishedAt = DateTime.UtcNow;

        await _articleRepository.UpdateAsync(article);
       
    }
}
