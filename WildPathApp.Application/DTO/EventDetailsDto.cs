using WildPathApp.Core.Domain.Models;

namespace WildPathApp.Application.DTO;

public class EventDetailsDto
{
    public Event Event { get; set; } = new Event();
    public List<EventsCategories> Categories { get; set; } = [];
}
