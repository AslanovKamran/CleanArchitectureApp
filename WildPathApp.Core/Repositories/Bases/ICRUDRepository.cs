namespace WildPathApp.Core.Repositories.Bases;

public interface ICRUDRepository<T>
{
    Task<int> Create(T entity);
    Task<int> Update(T entity);
    Task<int> Delete(int id);
    Task<T> Get(Guid id);
    Task<List<T>> GetAll();
}
