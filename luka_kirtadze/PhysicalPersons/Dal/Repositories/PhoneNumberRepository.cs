using Dal.Contracts;
using Dal.Data;

namespace Dal.Repositories;

public class PhoneNumberRepository : Repository<PhoneNumber>,IPhoneNumberRepository
{
    public PhoneNumberRepository(PersonDbContext context) : base(context)
    {
    }
}