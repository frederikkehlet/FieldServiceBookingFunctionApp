using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using Domain.Extensions;

namespace Domain.Clients.Implementation
{
    public class GraphClient : IGraphClient
    {
        private string _tenantId;
        private string _clientId;
        private string _clientSecret;
        private string _scope;
        private const string baseUri = "https://graph.microsoft.com/v1.0/";

        private HttpClient _httpClient { get; set; }

        public GraphClient(string tenantId, string clientId, string clientSecret, string scope)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", GetAccessToken().Result);
            _httpClient.BaseAddress = new Uri(baseUri);
        }

        public async Task Delete(string resourceUri)
        {
            using (_httpClient)
            {
                try
                {
                    await _httpClient.DeleteAsync(resourceUri);
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"Error making DELETE request to {_httpClient.BaseAddress}{resourceUri}: {ex.Message}." +
                        $"\nStack trace: {ex.StackTrace}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error occurred: {ex.Message}." +
                                            $"\nStack trace: {ex.StackTrace}");
                }
            }
        }

        public async Task<T> Get<T>(string resourceUri)
        {
            using (_httpClient)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(resourceUri);
                    return await HandleResponse<T>(response);
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"Error making GET request to {_httpClient.BaseAddress}{resourceUri}: {ex.Message}." +
                        $"\nStack trace: {ex.StackTrace}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error occurred: {ex.Message}." +
                                            $"\nStack trace: {ex.StackTrace}");
                }
            }
        }

        public async Task<T> Patch<T>(string resourceUri, string content)
        {
            using (_httpClient)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.PatchAsync(resourceUri, 
                        new  StringContent(content, Encoding.UTF8, "application/json"));

                    return await HandleResponse<T>(response);
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"Error making PATCH request to {_httpClient.BaseAddress}{resourceUri}: {ex.Message}." +
                        $"\nStack trace: {ex.StackTrace}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error occurred: {ex.Message}." +
                        $"\nStack trace: {ex.StackTrace}");
                }
            }
        }

        public async Task<T> Post<T>(string resourceUri, string content)
        {
            using (_httpClient)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.PostAsync(resourceUri,
                        new StringContent(content, Encoding.UTF8, "application/json"));  
                    
                    return await HandleResponse<T>(response);
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"Error making POST request to {_httpClient.BaseAddress}{resourceUri}: {ex.Message}." +
                        $"\nStack trace: {ex.StackTrace}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error occurred: {ex.Message}." +
                        $"\nStack trace: {ex.StackTrace}");
                }
            }
        }

        private async Task<string> GetAccessToken()
        {
            string tokenUrl = $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token";

            using (var requestClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "scope", _scope },
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret }
                });

                var response = await requestClient.PostAsync(tokenUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();
                TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseString);
                return tokenResponse.AccessToken;
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            T responseType = JsonSerializer.Deserialize<T>(responseString);
            return responseType;
        }
    }
}
