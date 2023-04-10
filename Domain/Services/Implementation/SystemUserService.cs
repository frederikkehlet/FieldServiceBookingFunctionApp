using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Linq;
using System.Security.Cryptography;
using Tavis.UriTemplates;

namespace Domain.Services.Implementation
{
    public class SystemUserService : ISystemUserService
    {
        private readonly ServiceClient _client;

        public SystemUserService(ServiceClient client)
        {
            _client = client;
        }

        public SystemUser Get(Guid id)
        {
            using (var context = new MyCompanyContext(_client))
            {
                return context.SystemUserSet.Where(user => user.Id == id).FirstOrDefault();
            }
        }

        public SystemUser GetUserFor(Guid bookingId)
        {
            using (var context = new MyCompanyContext(_client))
            {
                var query = 
                    from booking in context.BookableResourceBookingSet
                    join resource in context.BookableResourceSet on booking.Resource.Id equals resource.Id
                    join user in context.SystemUserSet on resource.UserId.Id equals user.Id
                    where booking.BookableResourceBookingId == bookingId
                    select user;
                
                return query.FirstOrDefault();
            }
        }
    }
}
