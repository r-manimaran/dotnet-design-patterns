using ToDoApi.Events;

namespace ToDoApi.Aggregation
{
    public class TodoAggregate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        public void Apply(TodoCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Description = @event.Description;
            CreatedDate = @event.CreatedOn;
            IsCompleted = @event.IsCompleted;
        }

        public void Apply(TodoDeleted @event) => Id = @event.Id;
        public void Apply(TodoUpdated @event)
        {
            Name= @event.Name;
            Description = @event.Description;
            UpdatedDate = @event.UpdatedOn;
        }

        public void Apply(TodoCompleted @event)
        {
            UpdatedDate= @event.UpdatedOn;
            IsCompleted= @event.IsCompleted;
        }

    }
}
