using Dal.Contracts;
using Dal.Data;

namespace Dal.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly PersonDbContext _context;
    public IPersonRepository  Persons { get; }
    public ICityRepository  Cities { get; }
    public IPhoneNumberRepository  PhoneNumbers { get; }
    public  IRelationshipRepository Relationships { get; }
  
    public UnitOfWork(PersonDbContext context, IPersonRepository personRepository, ICityRepository cityRepository,IPhoneNumberRepository phoneNumberRepository, IRelationshipRepository relationship)
    {
        _context = context;
        Persons = personRepository;
        Cities = cityRepository;
        PhoneNumbers = phoneNumberRepository;
        Relationships = relationship;
    }
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}