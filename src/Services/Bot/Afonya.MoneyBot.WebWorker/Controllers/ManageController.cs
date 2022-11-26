using Afonya.MoneyBot.Interfaces.Dto;
using Afonya.MoneyBot.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Afonya.MoneyBot.WebWorker.Controllers;

[ApiController]
[Authorize]
public class ManageController : ControllerBase
{
    private readonly ILogger<ManageController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUserService _userService;
    private readonly BotConfiguration _botConfig;
    private readonly bool _useReverseProxy;
    private readonly string _subdir;
    
    public ManageController(ILogger<ManageController> logger, 
        ICategoryService categoryService, IConfiguration configuration, 
        ITelegramBotClient telegramBotClient, IUserService userService)
    {
        _logger = logger;
        _categoryService = categoryService;
        _telegramBotClient = telegramBotClient;
        _userService = userService;
        _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        _ = bool.TryParse(configuration["USE_REVERSE_PROXY"], out _useReverseProxy);
        _subdir = configuration["SUBDIR_PATH"] ?? "";
    }
    
    [HttpGet("api/category")]
    public IReadOnlyCollection<CategoryDto> GetCategories([FromQuery, SwaggerParameter("Включая неактивные")]bool all = false)
    {
        var data = _categoryService.Get(all);
        return data;
    }
    
    [HttpPost("api/category")]
    public CategoryDto PostCategory(CategoryDto category)
    {
        var data = _categoryService.Create(category);
        return data;
    }
    
    [HttpPut("api/category")]
    public CategoryDto PutCategory(CategoryDto category)
    {
        var data = _categoryService.Update(category);
        return data;
    }
    
    [HttpDelete("api/category")]
    public bool DeleteCategory(string id)
    {
        var data = _categoryService.Delete(id);
        return data;
    }
    
    [HttpPost("bot/status")]
    public async Task<WebhookInfo> StatusBot()
    {
        var result = await _telegramBotClient.GetWebhookInfoAsync();
        return result;
    }
    
    [HttpPost("bot/start")]
    public async Task<bool> StartBot()
    {
        var webHookAddress = _useReverseProxy ? 
            $"{_botConfig.HostAddress}{_subdir}/bot/{_botConfig.BotToken}" : 
            $"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
        _logger.LogInformation("Set webHook: {WebHookAddress}", webHookAddress);
        await _telegramBotClient.SetWebhookAsync(url: webHookAddress, 
            allowedUpdates: Array.Empty<UpdateType>());
        return true;
    }
    
    [HttpPost("bot/stop")]
    public async Task<bool> StopBot()
    {
        _logger.LogInformation("Delete webHook");
        await _telegramBotClient.DeleteWebhookAsync();
        return true;
    }
    
    [HttpGet("api/user")]
    public IReadOnlyCollection<UserDto> GetUsers()
    {
        var data = _userService.Get();
        return data;
    }
    
    [HttpPost("api/user")]
    public UserDto PostUser(UserDto user)
    {
        var data = _userService.Create(user);
        return data;
    }
    
    [HttpDelete("api/user")]
    public bool DeleteUser(string id)
    {
        var data = _categoryService.Delete(id);
        return data;
    }
}