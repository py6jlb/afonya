using Bot.Domain.Entities;
using Bot.Interfaces.Dto;

namespace Bot.Interfaces.Services;

public interface ICategoryService
{
    IEnumerable<CategoryDto> Get(bool onlyActive = true);
}