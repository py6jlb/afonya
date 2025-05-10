using System;
using Shared.Contracts;

namespace WebUI.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>?> LoadCategories(bool all = false);
}
