namespace ToDoApi.DTOs;

public record UpdateTodo(Guid Id, string TaskName, string? Description);
