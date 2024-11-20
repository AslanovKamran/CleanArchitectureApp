namespace WildPathApp.Core.Domain.Models;

public class EventsCategories
{
    public int EventId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
}
