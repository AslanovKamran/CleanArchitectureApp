﻿namespace WildPathApp.Core.Domain.Models;
public class DifficultyLevel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
