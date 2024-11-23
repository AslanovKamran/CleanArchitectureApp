using WildPathApp.Core.Domain.Models;

namespace WildPathApp.Core.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync();
}
