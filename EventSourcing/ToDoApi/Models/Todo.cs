namespace ToDoApi.Models;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsDeleted { get; set; }
}
