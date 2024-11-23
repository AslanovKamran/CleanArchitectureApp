using WildPathApp.Core.Domain.Models;

namespace WildPathApp.Core.Repositories;

public interface IDifficultyLevelRepository
{
    Task<List<DifficultyLevel>> GetDifficultyLevelsAsync();
}
