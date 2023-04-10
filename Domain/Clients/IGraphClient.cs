using System.Threading.Tasks;

namespace Domain.Clients
{
    public interface IGraphClient
    {
        Task<T> Get<T>(string resourceUri);
        Task<T> Post<T>(string resourceUri, string content);
        Task<T> Patch<T>(string resourceUri, string content);
        Task Delete(string resourceUri);
    }
}
