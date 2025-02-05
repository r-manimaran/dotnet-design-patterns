using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Aggregation;
using ToDoApi.DTOs;
using ToDoApi.Events;
using ToDoApi.Models;

namespace ToDoApi.Endpoints;

public class TodoEndpoints
{
    private readonly IDocumentStore _documentStore;
    public TodoEndpoints(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    public static void MapToDoEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/Todos");
        
        var handler = app.ServiceProvider.GetRequiredService<TodoEndpoints>();
        
        group.MapPost("/", handler.CreateTodo).WithName("create");

        group.MapPut("/", handler.UpdateTodo).WithName("update");
        
        group.MapDelete("/{id}",handler.DeleteTodo).WithName("delete");

        group.MapPut("/{id}/complete", handler.CompleteTodo).WithName("complete");

        group.MapGet("/{id}/history", handler.GetTodoHistory).WithName("history");

        group.MapGet("/currentState/{id}", handler.GetCurrentTodoState).WithName("currentstate");
    }

    public async Task<IResult> CreateTodo([FromBody]CreateTodo todo)
    {
        var session = _documentStore.LightweightSession();
        Guid todoId = Guid.NewGuid();
        var todoCreated = new TodoCreated(todoId, todo.TaskName, todo.Description, DateTime.UtcNow);
        session.Events.StartStream<Todo>(todoId, todoCreated);
        await session.SaveChangesAsync();
        return Results.Ok(new { Id = todoId });
    }

    public async Task<IResult> UpdateTodo([FromBody] UpdateTodo todo)
    {
        var session = _documentStore.LightweightSession();
        var todoUpdated = new TodoUpdated(todo.Id, todo.TaskName, todo.Description,DateTime.UtcNow);
        session.Events.Append(todo.Id,todoUpdated);
        await session.SaveChangesAsync();

        return Results.Ok(todoUpdated);
    }   

    public async Task<IResult> CompleteTodo(Guid Id, [FromBody] CompleteTodo todo)
    {
        if (Id != todo.Id) return Results.BadRequest();

        var session = _documentStore.LightweightSession();
        var todoCompleted = new TodoCompleted(todo.Id, DateTime.UtcNow);
        session.Events.Append(todo.Id, todoCompleted);
        await session.SaveChangesAsync();

        return Results.Ok(todoCompleted);
    }

    public async Task<IResult> DeleteTodo([FromBody] DeleteTodo todo)
    {
        var session = _documentStore.LightweightSession();
        var todoDeleted = new TodoDeleted(todo.Id,DateTime.UtcNow);
        session.Events.Append(todo.Id, todoDeleted);
        await session.SaveChangesAsync();

        return Results.Ok(todoDeleted);
    }

    public async Task<IResult> GetTodoHistory(Guid Id)
    {
        var session = _documentStore.LightweightSession();
        var events = await session.Events.FetchStreamAsync(Id);
        return Results.Ok(events.Select(e => new
        {
            e.Version,
            e.Data,
            e.Sequence,
            e.StreamId,
            e.Timestamp
        }));
    }

    public async Task<IResult> GetCurrentTodoState(Guid id)
    {
        var session = _documentStore.LightweightSession();
        var state = await session.Events.AggregateStreamAsync<TodoAggregate>(id);
        return state != null && !state.IsDeleted ? Results.Ok(state) : Results.NotFound("Task not found"); ;
    }

}
