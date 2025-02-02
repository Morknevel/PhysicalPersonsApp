using Dal.Contracts;
using Dal.Data;
using Dal.Paging;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(PersonDbContext context) : base(context)
    {
    }

    public async Task RemovePersonAsync(int personId)
    {
        var person = await DbSet.FindAsync(personId);
        if (person == null)
        {
            throw new ArgumentNullException(nameof(person));
        }

        DbSet.Remove(person);
    }

    public async Task CreatePerson(Person person)
    {
        await CreateAsync(person);
    }

   
    public async Task<IEnumerable<Person>> QuickSearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<Person>();

        searchTerm = searchTerm.Trim().ToLower();

        return await DbSet
            .AsNoTracking()
            .Where(p => EF.Functions.Like(p.FirstName.ToLower(), $"%{searchTerm}%") || 
                        EF.Functions.Like(p.LastName.ToLower(), $"%{searchTerm}%") || 
                        EF.Functions.Like(p.IdNumber, $"%{searchTerm}%"))
            .ToListAsync();
    }
    public void UpdatePerson(Person person)
    {
        DbSet.Update(person);
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        return await GetAsync(p => p.PersonId == id, includeProperties: "City,PhoneNumbers");
    }
    public async Task<(IEnumerable<Person> Results, int TotalCount)> DetailedSearchAsync(
        PersonSearchParameters parameters,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = DbSet.AsNoTracking();

        query = ApplySearchFilters(query, parameters);

        var totalRecords = await query.CountAsync();

        var results = await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (results, totalRecords);
    }

    private static IQueryable<Person> ApplySearchFilters(
        IQueryable<Person> query,
        PersonSearchParameters parameters)
    {
        if (!string.IsNullOrWhiteSpace(parameters.FirstName))
            query = query.Where(p => EF.Functions.Like(p.FirstName.ToLower(), 
                $"%{parameters.FirstName.Trim().ToLower()}%"));

        if (!string.IsNullOrWhiteSpace(parameters.LastName))
            query = query.Where(p => EF.Functions.Like(p.LastName.ToLower(), 
                $"%{parameters.LastName.Trim().ToLower()}%"));

        if (parameters.Gender.HasValue)
            query = query.Where(p => p.Gender == parameters.Gender);

        if (!string.IsNullOrWhiteSpace(parameters.IdNumber))
            query = query.Where(p => p.IdNumber == parameters.IdNumber);

        if (parameters.StartBirthDate.HasValue)
            query = query.Where(p => p.Birthday >= parameters.StartBirthDate.Value.Date);

        if (parameters.EndBirthDate.HasValue)
            query = query.Where(p => p.Birthday <= parameters.EndBirthDate.Value.Date);

        if (parameters.CityId.HasValue)
            query = query.Where(p => p.CityId == parameters.CityId);

        return query;
    }
    public async Task<Person?> GetWithDetailsAsync(int personId)
    {
        var person = await DbSet
            .Include(p => p.City)
            .Include(p => p.PhoneNumbers)
            .FirstOrDefaultAsync(p => p.PersonId == personId);

        if (person == null) return null;

        return person;
    }
}