using System;
using Persistence.Database;
using Application.Articles.GetArticleById;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Persistence.Queries;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery,ArticleResponse?>
{
    private readonly AppDbContext _dbContext;

    public GetArticleByIdQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ArticleResponse?> Handle(GetArticleByIdQuery request, 
                                               CancellationToken cancellationToken)
    {
        var articleResponse = await _dbContext
                                .Articles
                                .AsNoTracking()
                                .Where(a => a.Id == request.Id)
                                .Select(a => new ArticleResponse
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    Content = a.Content,
                                    Tags = a.Tags,
                                    CreatedAt = a.CreatedAt,
                                    PublishedAt = a.PublishedAt,
                                })
                                .FirstOrDefaultAsync();
        return articleResponse;
    }
}
