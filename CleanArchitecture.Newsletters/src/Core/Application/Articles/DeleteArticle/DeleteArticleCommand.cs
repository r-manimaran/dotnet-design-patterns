using MediatR;

namespace Application.Articles.DeleteArticle;

public sealed record DeleteArticleCommand (Guid ArticleId) : IRequest;
