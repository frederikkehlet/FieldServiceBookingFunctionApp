using System;

namespace Domain.Services
{
    public interface ISystemUserService
    {
        SystemUser Get(Guid id);
        SystemUser GetUserFor(Guid bookingId);
    }
}
