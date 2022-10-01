using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ExternalConfig.Models;
using Microsoft.Extensions.Configuration;

namespace ExternalConfig;

public class ApiConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly Timer _timer;  
    private readonly ApiConfigurationSource _apiConfigurationSource;  
  
    public ApiConfigurationProvider(ApiConfigurationSource apiConfigurationSource)  
    {  
        _apiConfigurationSource = apiConfigurationSource;  
        _timer = new Timer(x => Load(),   
            null,   
            TimeSpan.FromSeconds(_apiConfigurationSource.Period),   
            TimeSpan.FromSeconds(_apiConfigurationSource.Period));  
    }  
  
    public void Dispose()  
    {  
        _timer?.Change(Timeout.Infinite, 0);  
        _timer?.Dispose();  
        Console.WriteLine("Dispose timer");  
    }  
  
    public override void Load()  
    {  
        try  
        {  
            var url = $"{_apiConfigurationSource.ReqUrl}?appName={_apiConfigurationSource.AppName}";

            using var client = new HttpClient();
            var resp = client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();  
            var res = resp.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); 
            
            var option = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };

            var config = JsonSerializer.Deserialize<ConfigResult>(res, option);  
  
            if (config is not null)  
            {  
                Data = config.Data;  
                OnReload();  
                Console.WriteLine($"update at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");  
                Console.WriteLine($"{res}");  
            }  
            else  
            {  
                CheckOptional();  
            }
        }  
        catch  
        {  
            CheckOptional();  
        }  
    }  
  
    private void CheckOptional()  
    {  
        if (!_apiConfigurationSource.Optional)  
        {  
            throw new Exception($"can not load config from {_apiConfigurationSource.ReqUrl}");  
        }  
    } 
}