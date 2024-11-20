using WildPathApp.Core.Domain.Models;
using WildPathApp.Application.DTO;

namespace WildPathApp.Application.Helpers.Mappers;

internal class EventsMapper
{
    public static EventDetailsDto MapEventToEventDetailsDto(Event eventData, List<EventsCategories> eventCategories)
    {
        var categoriesForEvent = eventCategories
            .Where(ec => ec.EventId == eventData.Id)
            .ToList();

        return new EventDetailsDto
        {
            Event = eventData,
            Categories = categoriesForEvent
        };
    }
}
