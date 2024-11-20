using WildPathApp.Core.Domain.Models;

namespace WildPathApp.Core.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync();
}
