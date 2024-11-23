namespace WildPathApp.Core.Domain.Models;

public class Event : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public int MaxParticipantsCount { get; set; }
    public int CurrentParticipantsCount { get; set; }
    public int DifficultyId { get; set; }
    public string DifficultyLevelName { get; set; } = string.Empty;
    public string DifficultyLevelDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;

}
