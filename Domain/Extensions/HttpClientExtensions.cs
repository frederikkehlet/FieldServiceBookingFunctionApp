using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(
            this HttpClient httpClient, string requestUri, HttpContent content)
        {
            HttpMethod method = new HttpMethod("PATCH");

            HttpRequestMessage request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            HttpResponseMessage response = await httpClient.SendAsync(request);
            return response;
        }
    }
}
