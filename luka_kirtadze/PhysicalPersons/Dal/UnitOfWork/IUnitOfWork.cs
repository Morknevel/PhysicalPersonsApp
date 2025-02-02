namespace Dal.Contracts;

public interface IUnitOfWork
{
    public IPersonRepository Persons { get; }
    public ICityRepository Cities { get; }
    public IPhoneNumberRepository PhoneNumbers{ get; }
    public IRelationshipRepository Relationships { get; }
    Task SaveAsync();
}