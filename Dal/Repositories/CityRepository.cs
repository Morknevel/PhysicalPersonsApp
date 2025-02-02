using Dal.Contracts;
using Dal.Data;
using Dal.Models;

namespace Dal.Repositories;

public class CityRepository : Repository<City>,ICityRepository
{
    public CityRepository(PersonDbContext context) : base(context)
    {
    }

    public async Task CreateCityAsync(City city)
    {
        await CreateAsync(city);
    }
}