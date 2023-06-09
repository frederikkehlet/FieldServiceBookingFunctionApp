﻿using Domain.Models;
using Domain.Services;
using Domain.Services.Implementation;
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
                },
                Categories = booking.ToCategory(),
                ShowAs = booking.ToShowAs()
            };
        }

        private static string[] ToCategory(this BookableResourceBooking booking)
        {
            switch (booking.BookingStatus.Name)
            {
                case "Planlagte":
                    return new string[] { "Blå kategori" };
                case "Rejse":
                    return new string[] { "Rød kategori" };
                case "I gang":
                    return new string[] { "Grøn kategori" };
                default:
                    return new string[] { };
            };
        }

        private static string? ToShowAs(this BookableResourceBooking booking)
        {
            switch (booking.BookingStatus.Name)
            {
                case "Fuldført":
                    return "free" ;
                default:
                    return null;
            };
        }

        public static bool HasChangedResource(this BookableResourceBooking booking, Guid oldResourceId)
        {
            return booking.Resource.Id != oldResourceId;
        }
    }
}
