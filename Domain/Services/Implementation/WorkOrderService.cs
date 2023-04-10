using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Linq;

namespace Domain.Services.Implementation
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ServiceClient _client;

        public WorkOrderService(ServiceClient client)
        {
            _client = client;
        }

        public msdyn_workorder Get(Guid id)
        {
            using (var context = new MyCompanyContext(_client))
            {
                return context.msdyn_workorderSet.Where(wo => wo.Id == id).FirstOrDefault();
            }
        }
    }
}
