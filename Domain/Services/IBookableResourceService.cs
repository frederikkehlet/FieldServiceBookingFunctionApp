using System;

namespace Domain.Services
{
    public interface IBookableResourceService
    {
        BookableResource Get(Guid id);
    }
}
