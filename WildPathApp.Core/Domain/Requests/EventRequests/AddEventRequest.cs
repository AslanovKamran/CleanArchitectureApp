namespace WildPathApp.Core.Domain.Requests.EventRequests;

//Data atrributes may be added here
public class AddEventRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public int MaxParticipantsCount { get; set; }
    public int CurrentParticipantsCount { get; set; }
    public int DifficultyId { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;
    public string CategoryIds { get; set; } = string.Empty; // Comma-separated list of category IDs
}
