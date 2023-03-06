using System.Net.Http.Json;
using Afonya.WebUi.HttpClients.Abstractions;

namespace Afonya.WebUi.HttpClients;

public class ManageService : IManageService
{
    private readonly HttpClient _httpClient;

    public ManageService(HttpClient httpClient, IConfiguration config)
    {
        var remoteServiceBaseUrl = config["BorWorkerUrl"];
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(remoteServiceBaseUrl);
    }
    
    public async Task<object?> GetStatus()
    {
        var data = await _httpClient.GetFromJsonAsync<object>("bot/status");
        return data;
    }

    public async Task<bool> StartBot()
    {
        var data = await _httpClient.GetFromJsonAsync<bool>("bot/start");
        return data;
    }

    public async Task<bool> StopBoot()
    {
        var data = await _httpClient.GetFromJsonAsync<bool>("bot/stop");
        return data;
    }
}