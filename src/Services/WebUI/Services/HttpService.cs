using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Shared.Contracts;
using WebUI.Services.Interfaces;

namespace WebUI.Services;

public class HttpService : IHttpService
{
    private HttpClient _httpClient;
    private NavigationManager _navigationManager;
    private ILocalStorageService _localStorageService;

    public HttpService(
       HttpClient httpClient,
       NavigationManager navigationManager,
       ILocalStorageService localStorageService
   )
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
    }

    public async Task<T?> Get<T>(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        return await SendRequest<T>(request);
    }

    public async Task<T?> Post<T>(string uri, object value)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        return await SendRequest<T>(request);
    }

    private async Task<T?> SendRequest<T>(HttpRequestMessage request)
    {
        // add basic auth header if user is logged in and request is to the api url
        var user = await _localStorageService.GetItem<AuthenticateResponse>("authenticateResponse");
        var isApiUrl = !request.RequestUri.IsAbsoluteUri;
        if (user != null && isApiUrl)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        using var response = await _httpClient.SendAsync(request);

        // auto logout on 401 response
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navigationManager.NavigateTo("logout");
            return default;
        }

        // throw exception on error response
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new Exception(error?["message"]);
        }
        // var jsonOpt = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        // var res =  await response.Content.ReadFromJsonAsync<T>(jsonOpt);
        var res =  await response.Content.ReadFromJsonAsync<T>();
        return res;
    }
}
