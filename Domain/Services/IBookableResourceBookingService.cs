using System;

namespace Domain.Services
{
    public interface IBookableResourceBookingService
    {
        BookableResourceBooking Get(Guid id);
        void Update(BookableResourceBooking bookableResourceBooking);
    }
}
