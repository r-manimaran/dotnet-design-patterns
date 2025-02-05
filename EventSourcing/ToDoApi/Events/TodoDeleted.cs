namespace ToDoApi.Events;

public record TodoDeleted(Guid Id, DateTime UpdatedOn, bool IsDeleted=true);



