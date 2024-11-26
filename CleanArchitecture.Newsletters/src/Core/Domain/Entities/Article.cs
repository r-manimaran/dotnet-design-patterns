using System;

namespace Domain.Entities;

public class Article
{
 public Guid Id { get; set; }

 public string Title { get; set; } =string.Empty;
 
 public string Content { get; set; } = string.Empty;
 
 public List<string> Tags { get; set; } = new();
 
 public DateTime CreatedAt { get; set; } 
 
 public DateTime? PublishedAt { get; set; }
}
