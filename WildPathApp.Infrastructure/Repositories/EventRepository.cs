using WildPathApp.Core.Domain.Requests.EventRequests;
using WildPathApp.Core.Domain.CustomExceptions;
using WildPathApp.Infrastructure.Database;
using WildPathApp.Core.Domain.Models;
using WildPathApp.Core.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace WildPathApp.Infrastructure.Repositories;
public class EventRepository(DatabaseConnection dbConnection) : IEventRepository
{
    #region Get List
    public async Task<(List<Event>, List<EventsCategories>)> GetEventsAsync()
    {
        using (var connection = dbConnection.CreateConnection())
        {
            var query = "GetEvents";
            var multi = await connection.QueryMultipleAsync(query, commandType: CommandType.StoredProcedure);

            // First result: List of Events
            var events = (await multi.ReadAsync<Event>()).ToList();

            // Second result: List of EventCategoryDto
            var categories = (await multi.ReadAsync<EventsCategories>()).ToList();

            return (events, categories);
        }
    }

    #endregion

    #region Get
    public async Task<(Event, List<EventsCategories>)> GetEventByIdAsync(int id)
    {
        using (var connection = dbConnection.CreateConnection())
        {
            var parameters = new DynamicParameters();
            var query = "GetEventById";
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            var multi = await connection.QueryMultipleAsync(query, parameters, commandType: CommandType.StoredProcedure);

            var eventData = await multi.ReadSingleOrDefaultAsync<Event>();
            var eventCategories = await multi.ReadAsync<EventsCategories>();

            if (eventData == null)
                throw new KeyNotFoundException($"Event with ID {id} not found.");

            return (eventData, eventCategories.AsList());
        }
    }

    #endregion

    #region Add
    public async Task<int> AddEventAsync(AddEventRequest request)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", request.Name, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("Description", request.Description, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("StartsAt", request.StartsAt, DbType.DateTime, direction: ParameterDirection.Input);
        parameters.Add("EndsAt", request.EndsAt, DbType.DateTime, direction: ParameterDirection.Input);
        parameters.Add("MaxParticipantsCount", request.MaxParticipantsCount, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("CurrentParticipantsCount", request.CurrentParticipantsCount, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("DifficultyId", request.DifficultyId, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("Price", request.Price, DbType.Decimal, direction: ParameterDirection.Input);
        parameters.Add("Location", request.Location, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("CategoryIds", request.CategoryIds, DbType.String, direction: ParameterDirection.Input);  // Comma-separated list of category Ids
        parameters.Add("EventId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        var query = "AddEvent";
        try
        {
            using (var connection = dbConnection.CreateConnection())
            {
                // Execute the stored procedure and get the EventId from the output parameter
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);

                // Return the EventId from the output parameter
                return parameters.Get<int>("EventId");
            }
        }
        catch (SqlException ex)
        {
            throw new DatabaseException($"An error occurred while adding the event to the database: {ex.Message}", ex);
        }
    }

    #endregion

    #region Update
    public async Task<int> UpdateEventAsync(UpdateEventRequest request)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", request.Id, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("Name", request.Name, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("Description", request.Description, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("StartsAt", request.StartsAt, DbType.DateTime, direction: ParameterDirection.Input);
        parameters.Add("EndsAt", request.EndsAt, DbType.DateTime, direction: ParameterDirection.Input);
        parameters.Add("MaxParticipantsCount", request.MaxParticipantsCount, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("CurrentParticipantsCount", request.CurrentParticipantsCount, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("DifficultyId", request.DifficultyId, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add("Price", request.Price, DbType.Decimal, direction: ParameterDirection.Input);
        parameters.Add("Location", request.Location, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("CategoryIds", request.CategoryIds, DbType.String, direction: ParameterDirection.Input);  // Comma-separated list of category Ids

        var query = "UpdateEvent";
        try
        {
            using (var connection = dbConnection.CreateConnection())
            {
                // Execute the stored procedure and get the EventId from the output parameter
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);

                // Return the EventId from the output parameter
                return request.Id;
            }
        }
        catch (SqlException ex)
        {
            throw new DatabaseException($"An error occurred while adding the event to the database: {ex.Message}", ex);
        }
    }

    #endregion

    #region MyRegion

    public async Task<int> DeleteEventAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32, direction: ParameterDirection.Input);
        var query = "DeleteEvent";

        try
        {
            using (var connection = dbConnection.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                return id;
            }
        }
        catch (SqlException ex)
        {
            throw new KeyNotFoundException(ex.Message, ex);
        }
    }

    #endregion
}
