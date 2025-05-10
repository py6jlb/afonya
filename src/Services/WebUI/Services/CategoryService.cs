using System;
using Shared.Contracts;
using WebUI.Services.Interfaces;

namespace WebUI.Services;

public class CategoryService : ICategoryService
{

    private IHttpService _httpService;

    public CategoryService(
        IHttpService httpService
    )
    {
        _httpService = httpService;
    }

    public async Task<IEnumerable<CategoryDto>?> LoadCategories(bool all = false)
    {
        var categories = await _httpService.Get<IEnumerable<CategoryDto>>($"Categories?all={all}");
        return categories;
    }
}
