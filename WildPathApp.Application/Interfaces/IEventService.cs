using WildPathApp.Core.Domain.Requests.EventRequests;
using WildPathApp.Application.DTO;

namespace WildPathApp.Application.Interfaces;

public interface IEventService
{
    Task<EventDetailsDto> GetEventByIdAsync(int id);
    Task<List<EventDetailsDto>> GetEventsAsync();

    Task<int> AddEventAsync(AddEventRequest request);
    Task<int> UpdateEventAsync(UpdateEventRequest request);
    Task<int> DeleteEventAsync(int id);
}
