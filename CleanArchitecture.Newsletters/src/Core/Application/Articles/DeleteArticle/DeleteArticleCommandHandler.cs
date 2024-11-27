using Application.Articles.DeleteArticle;
using Domain.Repositories;
using MediatR;

namespace Application.DeleteArticle;

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IArticleRepository _articleRepository;

    public DeleteArticleCommandHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.ArticleId);

        if (article == null)
        {
            throw new Exception("Article not found");
        }

        await _articleRepository.DeleteAsync(request.ArticleId);
    }
}
