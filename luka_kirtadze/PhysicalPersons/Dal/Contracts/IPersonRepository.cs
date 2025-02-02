using Dal.Paging;

namespace Dal.Contracts;

public interface IPersonRepository : IRepository<Person>
{
    void UpdatePerson(Person person);
    Task<Person> GetPersonByIdAsync(int id);
    Task<Person?> GetWithDetailsAsync(int id);
    Task RemovePersonAsync(int personId);
    Task CreatePerson(Person person);
    Task<IEnumerable<Person>> QuickSearchAsync(string searchTerm);

    Task<(IEnumerable<Person> Results, int TotalCount)> DetailedSearchAsync(
        PersonSearchParameters parameters,
        int pageNumber,
        int pageSize);
}