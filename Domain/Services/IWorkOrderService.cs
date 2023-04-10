using System;

namespace Domain.Services
{
    public interface IWorkOrderService
    {
        msdyn_workorder Get(Guid id);
    }
}
