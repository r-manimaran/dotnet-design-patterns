using System;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly AppDbContext _context;

    public ArticleRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> InsertAsync(Article article)
    {
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article.Id;
    }
    public async Task<Article?> GetByIdAsync(Guid id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
        return article;
    }

    public async Task UpdateAsync(Article article)
    {
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Articles
         .FindAsync(id);

        if (entity != null)
        {
            _context.Articles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
