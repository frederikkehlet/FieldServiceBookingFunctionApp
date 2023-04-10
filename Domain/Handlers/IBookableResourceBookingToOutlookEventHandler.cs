using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public interface IBookableResourceBookingToOutlookEventHandler
    {
        Task HandleCreate(PluginStepMessage message);
        Task HandleUpdate(PluginStepMessage message);
        Task HandleDelete(PluginStepMessage message);
    }
}
