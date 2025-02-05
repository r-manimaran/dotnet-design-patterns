namespace ToDoApi.Events;

public record TodoCreated(Guid Id, string Name, string? Description, DateTime CreatedOn, bool IsCompleted=false, bool IsDeleted=false);



