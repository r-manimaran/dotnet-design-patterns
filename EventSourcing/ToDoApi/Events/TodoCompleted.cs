namespace ToDoApi.Events;

public record TodoCompleted(Guid Id, DateTime UpdatedOn, bool IsCompleted=true);



