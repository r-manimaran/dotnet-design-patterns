using System;
using MediatR;
using Application.Articles.CreateArticle;
using Application.Articles.GetArticleById;
using Application.Articles.PublishArticle;
using Application.Articles.DeleteArticle;
using Application.DeleteArticle;

namespace Api.Endpoints;

public static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this IEndpointRouteBuilder app){
        app.MapGet("api/articles/{id}", async(Guid id, ISender sender) => {

            var query = new GetArticleByIdQuery(id);
            var result = await sender.Send(query);
            if(result == null){
                return Results.NotFound();
            }
            return Results.Ok(result);
        });

        app.MapPost("api/articles", async(CreateArticleRequest request, ISender sender) => {
            var articleId = await sender.Send(
                new CreateArticleCommand(request.Title, request.Content, request.Tags));

            return Results.Created($"api/articles/{articleId}", articleId);
        });

        app.MapPut("api/articles/{id}", async(Guid id, ISender sender) => {
            await sender.Send(new PublishArticleCommand(id));
        });

        app.MapDelete("api/articles/{id}", async(Guid id, ISender sender) => {
            await sender.Send(new DeleteArticleCommand(id));
        });
    }
}
