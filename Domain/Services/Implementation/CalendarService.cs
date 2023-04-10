using Domain.Clients;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services.Implementation
{
    public class CalendarService : ICalendarService
    {
        private readonly IGraphClient _graphClient;
        private readonly ILogger<ICalendarService> _logger;

        public CalendarService(IGraphClient graphClient, ILogger<ICalendarService> logger)
        {
            _graphClient = graphClient;
            _logger = logger;
        }

        public async Task<Event> CreateEvent(Guid userActiveDirectoryObjectId, Event calendarEvent)
        {
            _logger.LogDebug($"Invoking {nameof(CalendarService.CreateEvent)}.");

            return await _graphClient.Post<Event>(
                $"users/{userActiveDirectoryObjectId}/events",
                JsonSerializer.Serialize(calendarEvent));
        }

        public async Task<Event> UpdateEvent(Guid userActiveDirectoryObjectId, string eventId, Event calendarEvent)
        {
            _logger.LogDebug($"Invoking {nameof(CalendarService.UpdateEvent)}.");

            return await _graphClient.Patch<Event>(
                $"users/{userActiveDirectoryObjectId}/events/{eventId}", 
                JsonSerializer.Serialize(calendarEvent));
        }

        public async Task DeleteEvent(Guid userActiveDirectoryObjectId, string eventId)
        {
            _logger.LogDebug($"Invoking {nameof(CalendarService.DeleteEvent)}.");

            await _graphClient.Delete($"users/{userActiveDirectoryObjectId}/events/{eventId}");
        }       
    }
}
