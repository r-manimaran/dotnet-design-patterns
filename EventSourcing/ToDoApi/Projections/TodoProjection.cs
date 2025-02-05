using Marten.Events.Aggregation;
using ToDoApi.Aggregation;
using ToDoApi.Events;

namespace ToDoApi.Projections;

public class TodoProjection: SingleStreamProjection<TodoAggregate>
{
    public void Apply(TodoCreated @event, TodoAggregate todoAggregate)
    {
        todoAggregate.Id = @event.Id;
        todoAggregate.Name = @event.Name;
    }

    public void Apply(TodoUpdated @event, TodoAggregate todoAggregate)
    {
        todoAggregate.Name = @event.Name;
        todoAggregate.Description = @event.Description;
    }

    public void Apply(TodoCompleted @event, TodoAggregate todoAggregate)
    {
        todoAggregate.IsCompleted = @event.IsCompleted;
    }
    public void Apply(TodoDeleted @event, TodoAggregate todoAggregate)
    {
        todoAggregate.IsDeleted = @event.IsDeleted;
    }
}
