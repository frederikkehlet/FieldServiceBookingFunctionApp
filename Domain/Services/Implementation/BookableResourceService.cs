using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Linq;

namespace Domain.Services.Implementation
{
    public class BookableResourceService : IBookableResourceService
    {
        private readonly ServiceClient _client;

        public BookableResourceService(ServiceClient client)
        {
            _client = client;
        }

        public BookableResource Get(Guid id)
        {
            using (var context = new MyCompanyContext(_client))
            {
                return context.BookableResourceSet.Where(resource => resource.Id == id).FirstOrDefault();
            }
        }
    }
}
