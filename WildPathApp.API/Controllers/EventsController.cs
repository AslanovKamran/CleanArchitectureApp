using WildPathApp.Core.Domain.Requests.EventRequests;
using WildPathApp.Core.Domain.CustomExceptions;
using WildPathApp.Application.Interfaces;
using WildPathApp.Core.Domain.Models;
using WildPathApp.Core.Domain.Api;
using WildPathApp.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WildPathApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(IEventService eventService) : ControllerBase
{

    #region Get List

    /// <summary>
    /// Retrieves a list of all events.
    /// </summary>
    /// <returns>A list of event details.</returns>


    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Event>> GetAll()
    {
        try
        {
            var events = await eventService.GetEventsAsync();
            var response = new ApiResponse<List<EventDetailsDto>>
            {
                Data = events,
                StatusCode = HttpStatusCode.OK,
                Message = "Events retrieved successfully",
                Success = true
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var message = $" An unexpected error occurred. Error details : {ex.Message}";
            var response = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, message, false);
            return StatusCode(500, response);
        }
    }

    #endregion

    #region Get 

    /// <summary>
    /// Retrieves the details of a specific event by its ID.
    /// </summary>
    /// <param name="id">The ID of the event to retrieve.</param>
    /// <returns>The details of the specified event.</returns>

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var eventDetails = await eventService.GetEventByIdAsync(id);
            var response = new ApiResponse<EventDetailsDto>
            {
                Data = eventDetails,
                StatusCode = HttpStatusCode.OK,
                Message = "Event retrieved successfully",
                Success = true
            };
            return Ok(response);

        }

        #region Catch blocks

        catch (KeyNotFoundException ex)
        {
            var response = new ApiResponse<object>(null, HttpStatusCode.NotFound, ex.Message, false);
            return NotFound(response);
        }
        catch (ApplicationException ex)
        {
            var response = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, ex.Message, false);
            return StatusCode(500, response);
        }
        catch (Exception ex)
        {
            var message = $" An unexpected error occurred. Error details : {ex.Message}";
            var response = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, message, false);
            return StatusCode(500, response);
        }

        #endregion
    }

    #endregion

    #region Add

    /// <summary>
    /// Creates a new event with the provided details.
    /// </summary>
    /// <param name="request">The details of the event to create.</param>
    /// <returns>The ID of the created event and its location.</returns>

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Add([FromForm] AddEventRequest request)
    {
        try
        {
            // Pass the AddEventRequest to the service layer to handle business logic
            var eventId = await eventService.AddEventAsync(request);

            // Return a successful response with the created event ID
            var resposne = new ApiResponse<int>(eventId, HttpStatusCode.Created, "Event successfully created.", true);
            return CreatedAtAction(nameof(Get), new { id = eventId }, resposne);
        }

        catch (DatabaseException ex)
        {
            // Return error response with the message
            return StatusCode(500, new ApiResponse<object>(null, HttpStatusCode.InternalServerError, ex.Message, false));
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            // Log.Error(ex, "Error adding event.");

            return StatusCode(500, new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "An unexpected error occurred: " + ex.Message, false));
        }
    }


    #endregion

    #region Update

    /// <summary>
    /// Updates the details of an existing event.
    /// </summary>
    /// <param name="request">The updated details of the event.</param>
    /// <returns>The ID of the updated event.</returns>

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update([FromForm] UpdateEventRequest request)
    {
        try
        {
            // Pass the AddEventRequest to the service layer to handle business logic
            var eventId = await eventService.UpdateEventAsync(request);

            // Return a successful response with the created event ID
            var resposne = new ApiResponse<int>(eventId, HttpStatusCode.Created, "Event successfully updated.", true);
            return Ok(resposne);
        }

        catch (DatabaseException ex)
        {
            // Return error response with the message
            return StatusCode(500, new ApiResponse<object>(null, HttpStatusCode.InternalServerError, ex.Message, false));
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            // Log.Error(ex, "Error adding event.");

            return StatusCode(500, new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "An unexpected error occurred: " + ex.Message, false));
        }
    }
    #endregion

    #region Delete

    /// <summary>
    /// Deletes an event by its ID.
    /// </summary>
    /// <param name="id">The ID of the event to delete.</param>
    /// <returns>A message indicating the result of the deletion.</returns>

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleteingId = await eventService.DeleteEventAsync(id);
            var response = new ApiResponse<string>($"Deleted at Id = {deleteingId}", HttpStatusCode.OK, "Event successfully deleted.", true);
            return Ok(response);

        }

        catch (KeyNotFoundException ex)
        {
            var response = new ApiResponse<object>(null, HttpStatusCode.NotFound, ex.Message, false);
            return NotFound(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "An error occurred while deleting the event: " + ex.Message, false);
            return StatusCode(500, response);
        }
    }

    #endregion

}
