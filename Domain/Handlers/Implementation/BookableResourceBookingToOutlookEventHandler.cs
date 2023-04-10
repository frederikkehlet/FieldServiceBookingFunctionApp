using Domain.Extensions;
using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Handlers.Implementation
{
    public class BookableResourceBookingToOutlookEventHandler : IBookableResourceBookingToOutlookEventHandler
    {
        private readonly ILogger<IBookableResourceBookingToOutlookEventHandler> _logger;
        private readonly IBookableResourceBookingService _bookableResourceBookingService;
        private readonly ICalendarService _calendarService;
        private readonly ISystemUserService _systemUserService;
        private readonly IBookableResourceService _bookableResourceService;
        private readonly IWorkOrderService _workOrderService;

        public BookableResourceBookingToOutlookEventHandler(
            ILogger<IBookableResourceBookingToOutlookEventHandler> logger,
            IBookableResourceBookingService bookableResourceBookingService,
            ICalendarService calendarService,
            ISystemUserService systemUserService,
            IBookableResourceService bookableResourceService,
            IWorkOrderService workOrderService)
        {
            _logger = logger;
            _bookableResourceBookingService = bookableResourceBookingService;
            _calendarService = calendarService;
            _systemUserService = systemUserService;
            _bookableResourceService = bookableResourceService;
            _workOrderService = workOrderService;
        }

        public async Task HandleCreate(PluginStepMessage message)
        {
            _logger.LogInformation($"Running {nameof(BookableResourceBookingToOutlookEventHandler.HandleCreate)}.");

            BookableResourceBooking booking = _bookableResourceBookingService.Get(message.PrimaryEntityId);
            _logger.LogDebug($"Booking: {JsonSerializer.Serialize(booking)}");

            if (booking == null)
            {
                _logger.LogError("No booking was found. Returning.");
                return;
            }

            SystemUser user = _systemUserService.GetUserFor(booking.Id);
            _logger.LogDebug($"User: {JsonSerializer.Serialize(user)}");

            if (user?.AzureActiveDirectoryObjectId == null)
            {
                _logger.LogError("No user with Azure AD object id was found. Returning.");
                return;
            }

            msdyn_workorder workOrder = _workOrderService.Get(booking.msdyn_WorkOrder.Id);
            Event calendarEvent = booking.ToEvent(workOrder);
            _logger.LogDebug($"Event: {JsonSerializer.Serialize(calendarEvent)}");
            
            Event eventResult = await _calendarService.CreateEvent(
                (Guid)user.AzureActiveDirectoryObjectId, calendarEvent);

            if (eventResult.Id != null)
            {
                _bookableResourceBookingService.Update(new BookableResourceBooking()
                {
                    Id = message.PrimaryEntityId,
                    new_outlookcalendareventid = eventResult.Id
                });
            }
        }

        public async Task HandleUpdate(PluginStepMessage message)
        {
            _logger.LogInformation($"Running {nameof(BookableResourceBookingToOutlookEventHandler.HandleUpdate)}.");

            BookableResourceBooking booking = _bookableResourceBookingService.Get(message.PrimaryEntityId);
            _logger.LogDebug($"Booking: {JsonSerializer.Serialize(booking)}");
      
            Guid oldResourceId = JsonSerializer.Deserialize<Lookup>(
                message.GetPreEntityImage("PreImage")?.GetValue("resource").ToString()).Id;

            if (booking.HasChangedResource(oldResourceId))
            {
                _logger.LogDebug("Booking has changed resource.");

                await HandleDelete(message);
                await HandleCreate(message);
            }
            else
            {
                if (booking == null || booking.new_outlookcalendareventid == null)
                {
                    _logger.LogError("No booking with an outlook calendar event id was found. Returning.");
                    return;
                }

                SystemUser user = _systemUserService.GetUserFor(booking.Id);
                _logger.LogDebug($"User: {JsonSerializer.Serialize(user)}");

                if (user?.AzureActiveDirectoryObjectId == null)
                {
                    _logger.LogError("No user with Azure AD object id was found. Returning.");
                    return;
                }

                msdyn_workorder workOrder = _workOrderService.Get(booking.msdyn_WorkOrder.Id);
                Event calendarEvent = booking.ToEvent(workOrder);

                await _calendarService.UpdateEvent(
                    (Guid)user.AzureActiveDirectoryObjectId, 
                    booking.new_outlookcalendareventid, calendarEvent);
            } 
        }

        public async Task HandleDelete(PluginStepMessage message)
        {
            _logger.LogInformation($"Running {nameof(BookableResourceBookingToOutlookEventHandler.HandleDelete)}.");

            string calendarEventId = message.GetPreEntityImage("PreImage")?.GetValue("new_outlookcalendareventid")?.ToString();
            _logger.LogDebug($"Calendar event id: {calendarEventId}");

            if (calendarEventId == null)
            {
                _logger.LogError("No calendar event id was found for the booking. Returning.");
                return;
            }

            Guid resourceId = JsonSerializer.Deserialize<Lookup>(
                message.GetPreEntityImage("PreImage")?.GetValue("resource").ToString()).Id;
            _logger.LogDebug($"Resource id: {resourceId}");

            if (resourceId == null)
            {
                _logger.LogError("No resource id was found for the booking. Returning.");
                return;
            }

            BookableResource resource = _bookableResourceService.Get(resourceId);
            
            if (resource.UserId == null)
            {
                _logger.LogError("No user was found on the resource. Returning.");
                return;
            }
            
            SystemUser user = _systemUserService.Get(resource.UserId.Id);
            _logger.LogDebug($"User: {JsonSerializer.Serialize(user)}");

            if (user?.AzureActiveDirectoryObjectId == null)
            {
                _logger.LogError("No user with Azure AD object id was found. Returning.");
                return;
            }

            await _calendarService.DeleteEvent((Guid)user.AzureActiveDirectoryObjectId, calendarEventId);
        }
    }
}
