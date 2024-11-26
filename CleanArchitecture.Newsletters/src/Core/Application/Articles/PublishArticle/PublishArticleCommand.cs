using System;

namespace Application.Articles.PublishArticle;

public sealed record PublishArticleCommand(Guid Id) : IRequest;

