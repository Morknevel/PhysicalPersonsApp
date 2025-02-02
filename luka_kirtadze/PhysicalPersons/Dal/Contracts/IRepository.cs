using System.Linq.Expressions;

namespace Dal.Contracts;

public interface IRepository<T>
{
    Task CreateAsync(T? entity);
    void Remove(T? entity);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true,string? includeProperties = null);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    void RemoveRange(IEnumerable<T> entities);
}
    