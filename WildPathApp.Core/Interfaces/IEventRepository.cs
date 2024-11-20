using WildPathApp.Core.Domain.Requests.EventRequests;
using WildPathApp.Core.Domain.Models;

namespace WildPathApp.Core.Interfaces;

public interface IEventRepository
{
    Task<(Event, List<EventsCategories>)> GetEventByIdAsync(int id);

    Task<(List<Event>, List<EventsCategories>)> GetEventsAsync();
    Task<int> AddEventAsync(AddEventRequest request);

    Task<int> UpdateEventAsync(UpdateEventRequest request);
    Task<int> DeleteEventAsync(int id);
}
