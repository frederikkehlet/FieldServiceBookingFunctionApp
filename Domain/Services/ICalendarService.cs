using Domain.Models;
using System;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICalendarService
    {
        Task<Event> CreateEvent(Guid userActiveDirectoryObjectId, Event calendarEvent);
        Task<Event> UpdateEvent(Guid userActiveDirectoryObjectId, string eventId, Event calendarEvent);
        Task DeleteEvent(Guid userActiveDirectoryObjectId, string eventId);
    }
}
