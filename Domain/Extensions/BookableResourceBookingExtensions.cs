using Domain.Models;
using System;

namespace Domain.Extensions
{
    public static class BookableResourceBookingExtensions
    {
        public static Event ToEvent(this BookableResourceBooking booking, msdyn_workorder workOrder)
        {
            return new Event()
            {
                Subject = booking.Name,
                Start = new Time
                {
                    DateTime = booking.StartTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    TimeZone = "UTC"
                },
                End = new Time
                {
                    DateTime = booking.EndTime?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    TimeZone = "UTC"
                },
                Location = new Location
                {
                    Address = new PhysicalAddress
                    {
                        Street = workOrder.msdyn_Address1,
                        City = workOrder.msdyn_City,
                        PostalCode = workOrder.msdyn_PostalCode,
                        CountryOrRegion = workOrder.msdyn_Country,
                    },
                    DisplayName = workOrder.GetFormattedAddress()
                }
            };
        }

        public static bool HasChangedResource(this BookableResourceBooking booking, Guid oldResourceId)
        {
            return booking.Resource.Id != oldResourceId;
        }
    }
}
