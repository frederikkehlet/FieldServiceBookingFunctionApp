using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Linq;

namespace Domain.Services.Implementation
{
    public class BookableResourceBookingService : IBookableResourceBookingService
    {
        private readonly ServiceClient _client;

        public BookableResourceBookingService(ServiceClient client)
        {
            _client = client;
        }
        
        public BookableResourceBooking Get(Guid id)
        {
            using (var context = new MyCompanyContext(_client))
            {
                return context.BookableResourceBookingSet.Where(booking => booking.Id == id).FirstOrDefault();
            }
        }

        public void Update(BookableResourceBooking bookableResourceBooking)
        {
            _client.Update(bookableResourceBooking);
        }
    }
}
