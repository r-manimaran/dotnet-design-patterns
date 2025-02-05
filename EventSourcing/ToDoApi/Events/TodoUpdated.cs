namespace ToDoApi.Events;

public record TodoUpdated(Guid Id, string Name, string? Description, DateTime UpdatedOn);



