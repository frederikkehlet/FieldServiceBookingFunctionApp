using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
