using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.WebWorker.Auth;
using Common.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.WebWorker.Controllers;

[ApiController]
//[Authorize]
public class ManageController : ControllerBase
{
    private readonly ILogger<ManageController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUserService _userService;
    private readonly BotConfiguration _botConfig;
    private readonly ReverseProxyConfig _proxyConfig;
    
    public ManageController(ILogger<ManageController> logger, 
        ICategoryService categoryService, IOptions<BotConfiguration> botConfig,
        IOptions<ReverseProxyConfig> proxyConfig,
        ITelegramBotClient telegramBotClient, IUserService userService)
    {
        _logger = logger;
        _categoryService = categoryService;
        _telegramBotClient = telegramBotClient;
        _userService = userService;
        _botConfig = botConfig.Value;
        _proxyConfig = proxyConfig.Value;
    }
    
    [HttpGet("api/category")]
    [BasicAuth]
    public IReadOnlyCollection<CategoryDto> GetCategories([FromQuery, SwaggerParameter("Включая неактивные")]bool all = false)
    {
        var data = _categoryService.Get(all);
        return data;
    }
    
    [HttpPost("api/category")]
    [BasicAuth]
    public CategoryDto PostCategory(CategoryDto category)
    {
        var data = _categoryService.Create(category);
        return data;
    }
    
    [HttpPut("api/category")]
    [BasicAuth]
    public CategoryDto PutCategory(CategoryDto category)
    {
        var data = _categoryService.Update(category);
        return data;
    }
    
    [HttpDelete("api/category")]
    [BasicAuth]
    public bool DeleteCategory(string id)
    {
        var data = _categoryService.Delete(id);
        return data;
    }
    
    [HttpPost("bot/status")]
    [BasicAuth]
    public async Task<WebhookInfo> StatusBot()
    {
        var result = await _telegramBotClient.GetWebhookInfoAsync();
        return result;
    }
    
    [HttpPost("bot/start")]
    [BasicAuth]
    public async Task<bool> StartBot()
    {
        var webHookAddress = _proxyConfig?.UseReverseProxy ?? false ? 
            $"{_botConfig.HostAddress}{_proxyConfig?.SubDir ?? ""}/bot/{_botConfig.BotToken}" : 
            $"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
        _logger.LogInformation("Set webHook: {WebHookAddress}", webHookAddress);
        await _telegramBotClient.SetWebhookAsync(url: webHookAddress, 
            allowedUpdates: Array.Empty<UpdateType>());
        return true;
    }
    
    [HttpPost("bot/stop")]
    [BasicAuth]
    public async Task<bool> StopBot()
    {
        _logger.LogInformation("Delete webHook");
        await _telegramBotClient.DeleteWebhookAsync();
        return true;
    }
    
    [HttpGet("api/user")]
    [BasicAuth]
    public IReadOnlyCollection<UserDto> GetUsers()
    {
        var data = _userService.Get();
        return data;
    }
    
    [HttpPost("api/user")]
    [BasicAuth]
    public UserDto PostUser(UserDto user)
    {
        var data = _userService.Create(user);
        return data;
    }
    
    [HttpDelete("api/user")]
    [BasicAuth]
    public bool DeleteUser(string id)
    {
        var data = _categoryService.Delete(id);
        return data;
    }
}