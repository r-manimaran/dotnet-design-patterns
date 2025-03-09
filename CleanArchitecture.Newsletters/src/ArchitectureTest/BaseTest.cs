using Api;
using Application.Articles.CreateArticle;
using Domain.Entities;
using Persistence.Repositories;
using System.Reflection;

namespace ArchitectureTest;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Article).Assembly;
    protected static readonly Assembly InfrastrucureAssembly = typeof(ArticleRepository).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(CreateArticleCommand).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
