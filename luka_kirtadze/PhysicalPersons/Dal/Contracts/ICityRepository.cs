using Dal.Models;

namespace Dal.Contracts;

public interface ICityRepository : IRepository<City>
{
    Task CreateCityAsync(City city);
}