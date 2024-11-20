using WildPathApp.Core.Domain.Models;

namespace WildPathApp.Core.Interfaces;

public interface IDifficultyLevelRepository
{
    Task<List<DifficultyLevel>> GetDifficultyLevelsAsync();
}
