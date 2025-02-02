using System.Linq.Expressions;
using Dal.Contracts;
using Dal.Data;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private PersonDbContext _context;
    internal DbSet<T> DbSet;

    public Repository(PersonDbContext context)
    {
        _context = context;
        DbSet = _context.Set<T>();
    }

    public async Task CreateAsync(T? entity)
    {
        await DbSet.AddAsync(entity);
    }

    public  void Remove(T? entity)
    {
        if (entity != null) DbSet.Remove(entity);
    }

    public async  Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true,
        string? includeProperties = null)
    {
        IQueryable<T> query = DbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }
    

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

}